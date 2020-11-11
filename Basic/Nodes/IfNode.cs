using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class IfNode : Node
    {
        public List<IfCase> IfCases { get; set; }
        public ElseCase ElseCase { get; set; }
        public IfNode(List<IfCase> ifCases, ElseCase elseCase)
        {
            IfCases = ifCases;
            ElseCase = elseCase;
        }
    }

    public class IfCase
    {
        public Node Condition { get; set; }
        public Node Expr { get; set; }
        public IfCase(Node condition, Node expr)
        {
            Condition = condition;
            Expr = expr;
        }
    }

    public class ElseCase
    {
        public Node Expr { get; set; }
        public ElseCase(Node expr)
        {
            Expr = expr;
        }
    }
}
