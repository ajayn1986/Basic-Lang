namespace Basic
{
    public class NumberNode : Node
    {
        public Token Tok { get; set; }
        public NumberNode(Token tok)
        {
            Tok = tok;
        }
        public override string ToString()
        {
            return Tok.ToString();
        }
    }
    public class VarAccessNode : Node
    {
        public Token VarNameTok { get; set; }
        public VarAccessNode(Token tok)
        {
            VarNameTok = tok;
        }
        public override string ToString()
        {
            return VarNameTok.ToString();
        }
    }
    public class VarAssignmentNode : Node
    {
        public Token VarNameTok { get; set; }
        public Node ValueNode { get; private set; }
        
        public VarAssignmentNode(Token varNameTok, Node valueNode)
        {
            VarNameTok = varNameTok;
            ValueNode = valueNode;
        }
        public override string ToString()
        {
            return $"{VarNameTok}, {ValueNode}";
        }
    }
}
