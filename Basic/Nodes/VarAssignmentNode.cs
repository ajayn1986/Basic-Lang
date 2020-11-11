namespace Basic
{
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
