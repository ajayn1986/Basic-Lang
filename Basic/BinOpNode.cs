namespace Basic
{
    public class BinOpNode : Node
    {
        public Node Left { get; set; }
        public Token Op_tok { get; set; }
        public Node Right { get; set; }
        public BinOpNode(Node left, Token op_tok, Node right)
        {
            Left = left;
            Op_tok = op_tok;
            Right = right;
        }
        public override string ToString()
        {
            return $"({Left}, {Op_tok}, {Right})";
        }
    }    
}
