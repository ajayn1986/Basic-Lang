namespace Basic
{
    public class ForNode : Node
    {
        public Token identifierToken;
        public Node StartValueNode;
        public Node EndValueNode;
        public Node StepValueNode;
        public Node BodyNode;

        public ForNode(Token identifierToken, Node startValueNode, Node endValueNode, Node stepValueNode, Node bodyNode)
        {
            this.identifierToken = identifierToken;
            this.StartValueNode = startValueNode;
            this.EndValueNode = endValueNode;
            this.StepValueNode = stepValueNode;
            this.BodyNode = bodyNode;
        }
    }
}