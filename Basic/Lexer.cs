using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Lexer
    {
        private string Text { get; set; }
        private Position pos { get; set; }
        private string current_char { get; set; }
        private string Digits = "0123456789";
        private string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string AlphaNum;
        private string[] Keywords = new string[] {
            "var"
        };

        public Lexer(string text, string fileName)
        {
            AlphaNum = Letters + Digits;
            Text = text;
            pos = new Position(-1, 0, -1, fileName, text);
            Advance();
        }

        public void Advance()
        {
            pos.Advance(current_char);
            if (pos.Idx < Text.Length) current_char = Text[pos.Idx].ToString();
            else current_char = null;
        }

        public Tuple<PythonCollection<Token>, CompilationError> MakeTokens()
        {
            PythonCollection<Token> tokens = new PythonCollection<Token>();
            while (current_char != null)
            {
                if (string.IsNullOrWhiteSpace(current_char)) Advance();
                else if (Digits.Contains(current_char))
                {
                    tokens.Append(GetNumber());
                }
                else if (Letters.Contains(current_char))
                {
                    tokens.Append(MakeIdentifier());
                }
                else if (current_char == "/")
                {
                    tokens.Append(new Token(TokenType.Div, pos_start: pos));
                    Advance();
                }
                else if (current_char == "*")
                {
                    tokens.Append(new Token(TokenType.Mul, pos_start: pos));
                    Advance();
                }
                else if (current_char == "+")
                {
                    tokens.Append(new Token(TokenType.Plus, pos_start: pos));
                    Advance();
                }
                else if (current_char == "-")
                {
                    tokens.Append(new Token(TokenType.Minus, pos_start: pos));
                    Advance();
                }
                else if (current_char == "*")
                {
                    tokens.Append(new Token(TokenType.Mul, pos_start: pos));
                    Advance();
                }
                else if (current_char == "^")
                {
                    tokens.Append(new Token(TokenType.Pow, pos_start: pos));
                    Advance();
                }
                else if (current_char == "=")
                {
                    tokens.Append(new Token(TokenType.EQ, pos_start: pos));
                    Advance();
                }
                else if (current_char == "(")
                {
                    tokens.Append(new Token(TokenType.LParen, pos_start: pos));
                    Advance();
                }
                else if (current_char == ")")
                {
                    tokens.Append(new Token(TokenType.RParen, pos_start: pos));
                    Advance();
                }
                else
                {
                    var cur_char = current_char;
                    var start = pos.Copy();
                    Advance();
                    return new Tuple<PythonCollection<Token>, CompilationError>(null, new InvalidCharError("Invalid char '" + cur_char + "'", start, pos));
                }
            }
            tokens.Append(new Token(TokenType.EOF, pos_start: pos));
            return new Tuple<PythonCollection<Token>, CompilationError>(tokens, null);
        }

        private Token MakeIdentifier()
        {
            string identifier = "";
            var start = pos.Copy();
            while (current_char != null && (AlphaNum).Contains(current_char))
            {
                identifier += current_char;
                Advance();
            }
            if (Keywords.Contains(identifier))
                return new Token(TokenType.KEYWORD, identifier, start, pos);
            return new Token(TokenType.INDENTIFIER, identifier, start, pos);
        }

        private Token GetNumber()
        {
            string numStr = "";
            int dots = 0;
            var start = pos.Copy();
            while (current_char != null && (Digits + ".").Contains(current_char))
            {
                if (current_char == ".")
                {
                    if (dots > 0) break;
                    numStr += ".";
                    dots++;
                }
                else
                {
                    numStr += current_char;
                }
                Advance();
            }
            if (dots == 0) return new Token(TokenType.Int, Convert.ToInt32(numStr), start, pos);
            return new Token(TokenType.Float, Convert.ToDouble(numStr), start, pos);
        }
    }
}
