namespace Basic
{
    public class Token
    {
        public TokenType Type { get; set; }
        public dynamic Value { get; set; }
        public Position Pos_Start { get; private set; }
        public Position Pos_End { get; private set; }

        public Token(TokenType type, dynamic value = null, Position pos_start = null, Position pos_end = null)
        {
            Type = type;
            Value = value;
            if (pos_start != null)
            {
                Pos_Start = pos_start.Copy();
                Pos_End = pos_start.Copy();
                Pos_End.Advance();
            }
            if (pos_end != null)
                Pos_End = pos_end.Copy();
        }

        public bool Matches(TokenType type, dynamic value)
        {
            return this.Type == type && Value == value;
        }
        public override string ToString()
        {
            if (Value == null) return Type.ToString();
            return $"{Type}:{Value}";
        }

        public override bool Equals(object obj)
        {
            if (obj is TokenType) return this.Type == (TokenType)obj && this.Value == null;
            if (obj is Token) return ((Token)obj).Matches(this.Type, this.Value);
            return false;
        }

        public static implicit operator Token(TokenType type)
        {
            return new Token(type);
        }
    }
}
