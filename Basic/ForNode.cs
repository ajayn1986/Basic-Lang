namespace Basic
{
    public class ForNode : Node
    {        
        public Node StartValueNode;
        public Node EndValueNode;
        public Node StepValueNode;
        public Node BodyNode;

        public ForNode(Node startValueNode, Node endValueNode, Node stepValueNode, Node bodyNode)
        {            
            this.StartValueNode = startValueNode;
            this.EndValueNode = endValueNode;
            this.StepValueNode = stepValueNode;
            this.BodyNode = bodyNode;
        }
    }
}