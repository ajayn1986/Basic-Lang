using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Parser
    {
        private PythonCollection<Token> Tokens;
        private int Tok_idx;
        private Token current_token;

        public Parser(PythonCollection<Token> tokens)
        {
            Tokens = tokens;
            Tok_idx = -1;
            Advance();
        }

        private Token Advance()
        {
            Tok_idx++;
            if (Tok_idx < Tokens.Length)
            {
                current_token = Tokens[Tok_idx];
            }
            return current_token;
        }

        public ParseResult Parse()
        {
            var res = Expr();
            if (res.Error == null && current_token.Type != TokenType.EOF)
                return new InvalidSyntaxError(
                    "Expected '+', '-', '*', '/', '^', '==', '!=', '<', '>', <=', '>=', 'AND' or 'OR'",
                    current_token.Pos_Start, current_token.Pos_End
                );
            return res;

        }


        private ParseResult Factor()
        {
            var token = current_token;
            if (new[] { TokenType.Plus, TokenType.Minus }.Contains(token.Type))
            {
                Advance();
                var factorResult = Factor();
                if (factorResult.HasError()) return factorResult;
                return new UnaryOpNode(token, factorResult.Node).SetPos(token.Pos_Start, factorResult.Node.Pos_End);
            }
            return Power();
        }

        private ParseResult Power()
        {
            return BinOp(Atom, TokenType.Pow);
        }

        private ParseResult Term()
        {
            return BinOp(Factor, TokenType.Mul, TokenType.Div);
        }

        private ParseResult Expr()
        {
            if (current_token.Matches(TokenType.KEYWORD, "var"))
            {
                return GetVarDeclaration();
            }
            //return BinOp(Term, TokenType.Plus, TokenType.Minus);
            return BinCompOp(BoolCompExpr, new Token(TokenType.KEYWORD, "and"), new Token(TokenType.KEYWORD, "or"));
        }

        private ParseResult GetVarDeclaration()
        {
            Advance();
            if (current_token.Type != TokenType.INDENTIFIER)
                return new InvalidSyntaxError("Expected Identifier", current_token.Pos_Start, current_token.Pos_End);
            var var_name = current_token;
            Advance();
            if (current_token.Type != TokenType.EQ)
                return new InvalidSyntaxError("Expected '='", current_token.Pos_Start, current_token.Pos_End);
            Advance();
            var exprResult = Expr();
            if (exprResult.HasError()) return exprResult;
            return new VarDeclarationNode(var_name, exprResult.Node);
        }

        private ParseResult BoolCompExpr()
        {
            var binResult = BinOp(CompExpr, TokenType.EE, TokenType.NE);
            //if (binResult.HasError())
                //return new InvalidSyntaxError("Expected int, float, indentifier,'+','-', or 'not'", current_token.Pos_Start, current_token.Pos_End);
            return binResult;
        }

        private ParseResult CompExpr()
        {
            if (current_token.Matches(TokenType.KEYWORD, "not"))
            {
                var op_tok = current_token;
                Advance();
                var exprResult = CompExpr();
                if (exprResult.HasError()) return exprResult;
                return new UnaryOpNode(op_tok, exprResult.Node);
            }
            var binResult = BinOp(ArithExpr, TokenType.LT, TokenType.LTE, TokenType.GT, TokenType.GTE);
            //if (binResult.HasError())
                //return new InvalidSyntaxError("Expected int, float, indentifier,'+','-', or 'not'", current_token.Pos_Start, current_token.Pos_End);
            return binResult;
        }

        private ParseResult ArithExpr()
        {
            return BinOp(Term, TokenType.Plus, TokenType.Minus);
        }

        private ParseResult Atom()
        {
            var token = current_token;
            if (token.Type == TokenType.INDENTIFIER)
            {
                Advance();
                if (current_token.Equals(TokenType.EQ))
                {
                    Advance();
                    var expr = Expr();
                    if (expr.HasError()) return expr;
                    return new VarAssignmentNode(token, expr.Node).SetPos(token.Pos_Start, token.Pos_End);
                }
                else
                    return new VarAccessNode(token).SetPos(token.Pos_Start, token.Pos_End);
            }
            else if (new[] { TokenType.Int, TokenType.Float }.Contains(token.Type))
            {
                Advance();
                return new NumberNode(token).SetPos(token.Pos_Start, token.Pos_End);
            }
            else if (token.Type == TokenType.LParen)
            {
                Advance();
                var exprResult = Expr();
                if (exprResult.HasError()) return exprResult;
                if (current_token.Type == TokenType.RParen)
                {
                    Advance();
                    return exprResult;
                }
                else
                {
                    return new InvalidSyntaxError("Expected ')'", current_token.Pos_Start, current_token.Pos_End);
                }
            }
            else if (token.Equals(new Token(TokenType.KEYWORD, "if")))
            {
                return IfExpr();
            }
            else if (token.Equals(new Token(TokenType.KEYWORD, "for")))
            {
                return ForExpr();
            }
            else if (token.Equals(new Token(TokenType.KEYWORD, "while")))
            {
                return WhileExpr();
            }
            return new InvalidSyntaxError("Expected int, float, indentifier,'+','-', or '('", current_token.Pos_Start, current_token.Pos_End);
        }

        private ParseResult BinOp(Func<ParseResult> fn, params Token[] tokenTypes)
        {
            var leftParse = fn();
            if (leftParse.HasError())
                return leftParse;
            var left = leftParse.Node;
            while (tokenTypes.Contains(current_token.Type))
            {
                var op_tok = current_token;
                Advance();
                var rightParse = fn();
                if (rightParse.HasError()) return rightParse;
                var right = rightParse.Node;
                left = new BinOpNode(left, op_tok, right).SetPos(left.Pos_Start, right.Pos_End);
            }
            return left;
        }

        private ParseResult BinCompOp(Func<ParseResult> fn, params Token[] tokenTypes)
        {
            var leftParse = fn();
            if (leftParse.HasError())
                return leftParse;
            var left = leftParse.Node;
            while (tokenTypes.Contains(current_token))
            {
                var op_tok = current_token;
                Advance();
                var rightParse = fn();
                if (rightParse.HasError()) return rightParse;
                var right = rightParse.Node;
                left = new ConditionCompositeNode(left, op_tok, right).SetPos(left.Pos_Start, right.Pos_End);
            }
            return left;
        }

        private ParseResult WhileExpr()
        {
            var start = current_token.Pos_Start.Copy();
            Advance();
            var conditionExpr = Expr();
            if (conditionExpr.HasError())
                return conditionExpr;
            if (!current_token.Equals(new Token(TokenType.KEYWORD, "then")))
                return new InvalidSyntaxError("Expected 'then'", current_token.Pos_Start, current_token.Pos_End);
            Advance();

            var bodyResult = Expr();
            if (bodyResult.HasError()) return bodyResult;

            return new WhileNode(conditionExpr.Node, bodyResult.Node);
        }

        private ParseResult ForExpr()
        {
            var start = current_token.Pos_Start.Copy();
            Advance();
            Node initNode = null;
            if (current_token.Type == TokenType.INDENTIFIER)
            {
                Token idToken = current_token;
                Advance();
                if (current_token.Equals(TokenType.EQ))
                {
                    Advance();
                    var expr = Expr();
                    if (expr.HasError()) return expr;
                    initNode = new VarAssignmentNode(idToken, expr.Node).SetPos(idToken.Pos_Start, idToken.Pos_End);
                }
            }
            else if (current_token.Equals(new Token(TokenType.KEYWORD, "var")))
            {
                var declarationResult = GetVarDeclaration();
                if (declarationResult.HasError()) return declarationResult;
                initNode = declarationResult.Node;
            }
            else
                return new InvalidSyntaxError("Expected identifier or var", current_token.Pos_Start, current_token.Pos_End);            
            
            if (!current_token.Equals(new Token(TokenType.KEYWORD,"to")))
                return new InvalidSyntaxError("Expected 'to'", current_token.Pos_Start, current_token.Pos_End);
            Advance();

            var endResult = Expr();
            if (endResult.HasError()) return endResult;
            ParseResult stepResult = null;
            if (current_token.Equals(new Token(TokenType.KEYWORD, "step")))
            {
                Advance();
                stepResult = Expr();
                if (stepResult.HasError()) return stepResult;
            }
            if (!current_token.Equals(new Token(TokenType.KEYWORD, "then")))
                return new InvalidSyntaxError("Expected 'then'", current_token.Pos_Start, current_token.Pos_End);
            Advance();

            var bodyResult = Expr();
            if (bodyResult.HasError()) return bodyResult;

            return new ForNode(initNode, endResult.Node, stepResult?.Node, bodyResult.Node).SetPos(start, current_token.Pos_End);
        }


        private ParseResult IfExpr()
        {
            List<IfCase> cases = new List<IfCase>();
            ElseCase elseCase = null;
            var start = current_token.Pos_Start.Copy();
            Advance();
            var conditionResult = Expr();
            if (conditionResult.HasError())
            {
                return conditionResult;
            }

            if (!current_token.Equals(new Token(TokenType.KEYWORD, "then")))
                return new InvalidSyntaxError("Expected 'then'", current_token.Pos_Start, current_token.Pos_End);
            Advance();
            var ifResult = Expr();
            if (ifResult.HasError())
            {
                return ifResult;
            }
            cases.Add(new IfCase(conditionResult.Node, ifResult.Node));

            while (current_token.Equals(new Token(TokenType.KEYWORD, "elif")))
            {
                Advance();
                var elifConditionResult = Expr();
                if (elifConditionResult.HasError())
                {
                    return elifConditionResult;
                }

                if (!current_token.Equals(new Token(TokenType.KEYWORD, "then")))
                    return new InvalidSyntaxError("Expected 'then'", current_token.Pos_Start, current_token.Pos_End);
                Advance();
                var elifResult = Expr();
                if (elifResult.HasError())
                {
                    return elifResult;
                }
                cases.Add(new IfCase(elifConditionResult.Node, elifResult.Node));
            }

            if ((current_token.Equals(new Token(TokenType.KEYWORD, "else"))))
            {
                Advance();
                var elseResult = Expr();
                if (elseResult.HasError())
                {
                    return elseResult;
                }
                elseCase = new ElseCase(elseResult.Node);
            }
            
            return new IfNode(cases, elseCase).SetPos(start, elseCase?.Expr.Pos_End ?? cases[cases.Count - 1].Expr.Pos_End);
        }
    }
}
