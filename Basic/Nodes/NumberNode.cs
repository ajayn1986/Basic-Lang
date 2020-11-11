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
}
