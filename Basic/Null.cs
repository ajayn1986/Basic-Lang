namespace Basic
{
    public class Null
    {
        public Position Start_pos { get; set; } = null;
        public Position End_pos { get; set; } = null;
        public Context Context { get; set; }
        public Null()
        {
            this.SetContext();
        }
        public Null SetPos(Position start_pos, Position end_pos)
        {
            Start_pos = start_pos;
            End_pos = end_pos;
            return this;
        }
        public override string ToString()
        {
            return "null";
        }

        public override bool Equals(object obj)
        {
            return obj is Null;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public Null SetContext(Context context = null)
        {
            this.Context = context;
            return this;
        }
        public Null Copy()
        {
            Null copy = new Null();
            copy.SetPos(this.Start_pos, this.End_pos);
            copy.SetContext(this.Context);
            return copy;
        }
        public InterpreterResult ComparisonEq(dynamic val)
        {
            return new Binary(val is Null).SetContext(this.Context);
        }

        public InterpreterResult ComparisonNe(dynamic val)
        {
            return new Binary(!val is Null).SetContext(this.Context);
        }
    }
}
