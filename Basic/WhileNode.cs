namespace Basic
{
    public class WhileNode : Node
    {
        public Node ConditionNode { get; set; }
        public Node BodyNode { get; set; }

        public WhileNode(Node conditionNode, Node bodyNode)
        {
            this.ConditionNode = conditionNode;
            this.BodyNode = bodyNode;
        }
    }
}