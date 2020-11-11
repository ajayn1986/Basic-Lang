namespace Basic
{
    public abstract class Node
    {
        public Position Pos_Start { get; set; }
        public Position Pos_End { get; set; }

        public Node SetPos(Position pos_start, Position pos_end)
        {
            Pos_End = pos_end;
            Pos_Start = pos_start;
            return this;
        }
    }
}
