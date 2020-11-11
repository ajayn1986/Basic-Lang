namespace Basic
{
    public class VarDeclarationNode : Node
    {
        public Token VarNameTok { get; set; }
        public Node ValueNode { get; private set; }
        
        public VarDeclarationNode(Token varNameTok, Node valueNode)
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
