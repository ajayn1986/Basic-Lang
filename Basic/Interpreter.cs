using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Interpreter
    {
        public InterpreterResult Visit(Node node, Context context)
        {
            if (node is BinOpNode)
                return Visit(node as BinOpNode, context);
            if (node is ConditionCompositeNode)
                return Visit(node as ConditionCompositeNode, context);
            if (node is UnaryOpNode)
                return Visit(node as UnaryOpNode, context);
            if (node is NumberNode)
                return Visit(node as NumberNode, context);
            if (node is VarAssignmentNode)
                return Visit(node as VarAssignmentNode, context);
            if (node is VarAccessNode)
                return Visit(node as VarAccessNode, context);
            return null;
        }
        public InterpreterResult Visit(ConditionCompositeNode node, Context context)
        {
            InterpreterResult result = null;
            InterpreterResult leftResult = Visit(node.Left, context);
            if (leftResult.Error != null) return leftResult;
            if (!(leftResult.Result is Binary))
                return new InterpreterResult(null, new RuntimeError($"Expected boolean expression", node.Left.Pos_Start, node.Left.Pos_End, context));

            Binary left = leftResult.Result;
            InterpreterResult rightResult = Visit(node.Right, context);
            if (rightResult.Error != null) return rightResult;
            if (!(rightResult.Result is Binary))
                return new InterpreterResult(null, new RuntimeError($"Expected boolean expression", node.Right.Pos_Start, node.Right.Pos_End, context));

            Binary right = rightResult.Result;
            if (node.Op_tok.Matches(TokenType.KEYWORD, "and"))
                result = left.Anded_To(right);
            if (node.Op_tok.Matches(TokenType.KEYWORD, "or"))
                result = left.Ored_To(right);
            if (result.Error == null)
                result.Result.SetPos(left.Start_pos, right.End_pos);
            return result;
        }
        public InterpreterResult Visit(BinOpNode node, Context context)
        {
            InterpreterResult result = null;
            InterpreterResult leftResult = Visit(node.Left, context);
            if (leftResult.Error != null) return leftResult;
            if (leftResult.Result is Binary && new[] { TokenType.EE, TokenType.NE }.Contains(node.Op_tok.Type))
            {
                Binary left = leftResult.Result;
                InterpreterResult rightResult = Visit(node.Right, context);
                if (rightResult.Error != null) return rightResult;
                if (!(rightResult.Result is Binary))
                {
                    return new InterpreterResult(null, new RuntimeError($"Expected boolean expression", node.Right.Pos_Start, node.Right.Pos_End, context));
                }
                Binary right = rightResult.Result;
                if (node.Op_tok.Type == TokenType.EE)
                    result = left.ComparisonEq(right);
                if (node.Op_tok.Type == TokenType.NE)
                    result = left.ComparisonNe(right);
                if (result.Error == null)
                    result.Result.SetPos(left.Start_pos, right.End_pos);
                return result;
            }
            else
            {
                if (!(leftResult.Result is Number))
                {
                    return new InterpreterResult(null, new RuntimeError($"Expected number", node.Left.Pos_Start, node.Left.Pos_End, context));
                }
                Number left = leftResult.Result;
                InterpreterResult rightResult = Visit(node.Right, context);
                if (rightResult.Error != null) return rightResult;
                if (!(rightResult.Result is Number))
                {
                    return new InterpreterResult(null, new RuntimeError($"Expected number", node.Right.Pos_Start, node.Right.Pos_End, context));
                }
                Number right = rightResult.Result;
                if (node.Op_tok.Type == TokenType.Plus)
                    result = left.Added_To(right);
                if (node.Op_tok.Type == TokenType.Minus)
                    result = left.Subbed_by(right);
                if (node.Op_tok.Type == TokenType.Mul)
                    result = left.Multed_by(right);
                if (node.Op_tok.Type == TokenType.Div)
                    result = left.Dived_by(right);
                if (node.Op_tok.Type == TokenType.Pow)
                    result = left.Powed_by(right);
                if (node.Op_tok.Type == TokenType.LT)
                    result = left.ComparisonLt(right);
                if (node.Op_tok.Type == TokenType.GT)
                    result = left.ComparisonGt(right);
                if (node.Op_tok.Type == TokenType.LTE)
                    result = left.ComparisonLte(right);
                if (node.Op_tok.Type == TokenType.GTE)
                    result = left.ComparisonGte(right);
                if (node.Op_tok.Type == TokenType.EE)
                    result = left.ComparisonEq(right);
                if (node.Op_tok.Type == TokenType.NE)
                    result = left.ComparisonNe(right);
                if (result.Error == null)
                    result.Result.SetPos(left.Start_pos, right.End_pos);
                return result;
            }
        }
        public InterpreterResult Visit(UnaryOpNode node, Context context)
        {
            InterpreterResult numResult = Visit(node.Node, context);
            if (numResult.Error != null) return numResult;
            if (node.Op_tok.Matches(TokenType.KEYWORD, "not"))
            {
                if (!(numResult.Result is Binary))
                    return new InterpreterResult(null, new RuntimeError($"Expected boolean expression", node.Node.Pos_Start, node.Node.Pos_End, context));
                Binary bin = numResult.Result;
                return bin.SetPos(node.Pos_Start, node.Pos_End).Notted();
            }
            else
            {
                Number num = numResult.Result;
                if (node.Op_tok.Type == TokenType.Minus)
                    return num.SetPos(node.Pos_Start, node.Pos_End).Multed_by(new Number(-1));
                return num;
            }
        }
        public InterpreterResult Visit(NumberNode node, Context context)
        {
            return new InterpreterResult(new Number(node.Tok.Value).SetPos(node.Tok.Pos_Start, node.Tok.Pos_End).SetContext(context), null);
        }

        public InterpreterResult Visit(VarAssignmentNode node, Context context)
        {
            var result = Visit(node.ValueNode, context);
            if (result.Error != null) return result.Error;
            context.SymbolTable[node.VarNameTok.Value] = result.Result;
            return result;
        }

        public InterpreterResult Visit(VarAccessNode node, Context context)
        {
            var value = context.SymbolTable[node.VarNameTok.Value];
            if (value == null)
            {
                return new InterpreterResult(null, new RuntimeError($"{node.VarNameTok.Value} is not defined", node.Pos_Start, node.Pos_End, context));
            }
            if (value is Binary) return (value as Binary).Copy().SetPos(node.Pos_Start, node.Pos_End);
            return (value as Number).Copy().SetPos(node.Pos_Start, node.Pos_End);
        }
    }
}
