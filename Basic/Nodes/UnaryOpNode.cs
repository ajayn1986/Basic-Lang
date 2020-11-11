namespace Basic
{
    public class UnaryOpNode : Node
    {
        public Token Op_tok { get; set; }
        public Node Node { get; set; }
        public UnaryOpNode(Token op_tok, Node node)
        {
            Op_tok = op_tok;
            Node = node;
        }
        public override string ToString()
        {
            return $"({Op_tok}, {Node})";
        }
    }
}
