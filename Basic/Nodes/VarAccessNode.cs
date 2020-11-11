namespace Basic
{
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
}
