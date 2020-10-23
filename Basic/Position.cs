namespace Basic
{
    public class Position
    {
        public int Idx { get; private set; }
        public int Ln { get; private set; }
        public int Col { get; private set; }
        public string Fn { get; private set; }
        public string Text { get; private set; }

        public Position(int idx, int ln, int col, string fn, string text)
        {
            Idx = idx;
            Ln = ln;
            Col = col;
            Fn = fn;
            Text = text;
        }

        public Position Advance(string current_char = null)
        {
            Idx++;
            Col++;
            if (current_char == "\n")
            {
                Ln++;
                Col = 0;
            }
            return this;
        }

        public Position Copy()
        {
            return this.MemberwiseClone() as Position;
        }
    }
}
