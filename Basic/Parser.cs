﻿using System;
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
                return new ParseResult(null, new InvalidSyntaxError(
                    "Expected '+', '-', '*', '/', '^', '==', '!=', '<', '>', <=', '>=', 'AND' or 'OR'",
                    current_token.Pos_Start, current_token.Pos_End
                ));
            return res;

        }


        private ParseResult Factor()
        {
            var token = current_token;
            if (new[] { TokenType.Plus, TokenType.Minus }.Contains(token.Type))
            {
                Advance();
                var factorResult = Factor();
                if (factorResult.Error != null) return factorResult;
                return new ParseResult(new UnaryOpNode(token, factorResult.Node).SetPos(token.Pos_Start, factorResult.Node.Pos_End), null);
            }
            else if (token.Type == TokenType.INDENTIFIER)
            {
                Advance();
                return new ParseResult(new VarAccessNode(token).SetPos(token.Pos_Start, token.Pos_End), null);
            }
            else if (new[] { TokenType.Int, TokenType.Float }.Contains(token.Type))
            {
                Advance();
                return new ParseResult(new NumberNode(token).SetPos(token.Pos_Start, token.Pos_End), null);
            }
            else if (token.Type == TokenType.LParen)
            {
                Advance();
                var exprResult = Expr();
                if (exprResult.Error != null) return exprResult;
                if (current_token.Type == TokenType.RParen)
                {
                    Advance();
                    return exprResult;
                }
                else
                {
                    return new ParseResult(null, new InvalidSyntaxError("Expected ')'", current_token.Pos_Start, current_token.Pos_End));
                }
            }
            return new ParseResult(null, new InvalidSyntaxError("Expected Int or Float", current_token.Pos_Start, current_token.Pos_End));
        }

        private ParseResult Term()
        {
            return BinOp(Factor, TokenType.Mul, TokenType.Div, TokenType.Pow);
        }

        private ParseResult Expr()
        {
            if (current_token.Matches(TokenType.KEYWORD, "var"))
            {
                Advance();
                if (current_token.Type != TokenType.INDENTIFIER)
                    return new ParseResult(null, new InvalidSyntaxError("Expected Identifier", current_token.Pos_Start, current_token.Pos_End));
                var var_name = current_token;
                Advance();
                if (current_token.Type != TokenType.EQ)
                    return new ParseResult(null, new InvalidSyntaxError("Expected '='", current_token.Pos_Start, current_token.Pos_End));
                Advance();
                var exprResult = Expr();
                if (exprResult.Error != null) return exprResult;
                return new ParseResult(new VarAssignmentNode(var_name, exprResult.Node), null);
            }
            //return BinOp(Term, TokenType.Plus, TokenType.Minus);
            return BinCompOp(BoolCompExpr, new Token(TokenType.KEYWORD, "and"), new Token(TokenType.KEYWORD, "or"));
        }

        private ParseResult BoolCompExpr()
        {
            var binResult = BinOp(CompExpr, TokenType.EE, TokenType.NE);
            if (binResult.Error != null)
                return new ParseResult(null, new InvalidSyntaxError("Expected int, float, indentifier,'+','-', or 'not'", current_token.Pos_Start, current_token.Pos_End));
            return binResult;
        }

        private ParseResult CompExpr()
        {
            if (current_token.Matches(TokenType.KEYWORD, "not"))
            {
                var op_tok = current_token;
                Advance();
                var exprResult = CompExpr();
                if (exprResult.Error != null) return exprResult;
                return new ParseResult(new UnaryOpNode(op_tok, exprResult.Node), null);
            }
            var binResult = BinOp(ArithExpr, TokenType.LT, TokenType.LTE, TokenType.GT, TokenType.GTE);
            if (binResult.Error != null)
                return new ParseResult(null, new InvalidSyntaxError("Expected int, float, indentifier,'+','-', or 'not'", current_token.Pos_Start, current_token.Pos_End));
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
                return new ParseResult(new VarAccessNode(token).SetPos(token.Pos_Start, token.Pos_End), null);
            }
            else if (new[] { TokenType.Int, TokenType.Float }.Contains(token.Type))
            {
                Advance();
                return new ParseResult(new NumberNode(token).SetPos(token.Pos_Start, token.Pos_End), null);
            }
            else if (token.Type == TokenType.LParen)
            {
                Advance();
                var exprResult = Expr();
                if (exprResult.Error != null) return exprResult;
                if (current_token.Type == TokenType.RParen)
                {
                    Advance();
                    return exprResult;
                }
                else
                {
                    return new ParseResult(null, new InvalidSyntaxError("Expected ')'", current_token.Pos_Start, current_token.Pos_End));
                }
            }
            return new ParseResult(null, new InvalidSyntaxError("Expected int, float, indentifier,'+','-', or '('", current_token.Pos_Start, current_token.Pos_End));
        }

        private ParseResult BinOp(Func<ParseResult> fn, params Token[] tokenTypes)
        {
            var leftParse = fn();
            if (leftParse.Error != null)
                return leftParse;
            var left = leftParse.Node;
            while (tokenTypes.Contains(current_token.Type))
            {
                var op_tok = current_token;
                Advance();
                var rightParse = fn();
                if (rightParse.Error != null) return rightParse;
                var right = rightParse.Node;
                left = new BinOpNode(left, op_tok, right).SetPos(left.Pos_Start, right.Pos_End);
            }
            return new ParseResult(left, null);
        }

        private ParseResult BinCompOp(Func<ParseResult> fn, params Token[] tokenTypes)
        {
            var leftParse = fn();
            if (leftParse.Error != null)
                return leftParse;
            var left = leftParse.Node;
            while (tokenTypes.Contains(current_token))
            {
                var op_tok = current_token;
                Advance();
                var rightParse = fn();
                if (rightParse.Error != null) return rightParse;
                var right = rightParse.Node;
                left = new ConditionCompositeNode(left, op_tok, right).SetPos(left.Pos_Start, right.Pos_End);
            }
            return new ParseResult(left, null);
        }
    }
}