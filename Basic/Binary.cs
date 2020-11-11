using System;

namespace Basic
{
    public class Binary
    {
        bool Value;
        public Position Start_pos { get; set; } = null;
        public Position End_pos { get; set; } = null;
        public Context Context { get; set; }
        public Binary(bool value)
        {
            Value = value;
            this.SetContext();
        }
        public Binary SetPos(Position start_pos, Position end_pos)
        {
            Start_pos = start_pos;
            End_pos = end_pos;
            return this;
        }
        public Binary Copy()
        {
            Binary copy = new Binary(this.Value);
            copy.SetPos(this.Start_pos, this.End_pos);
            copy.SetContext(this.Context);
            return copy;

        }
        public override string ToString()
        {
            return Value.ToString();
        }
        public Binary SetContext(Context context = null)
        {
            this.Context = context;
            return this;
        }
        public InterpreterResult Anded_To(Binary val)
        {
            return new Binary(Value && val.Value).SetContext(this.Context);
        }
        public InterpreterResult Ored_To(Binary val)
        {
            return new Binary(Value || val.Value).SetContext(this.Context);
        }
        public InterpreterResult ComparisonEq(Binary val)
        {
            return new Binary(this.Value == val.Value).SetContext(this.Context);
        }

        public InterpreterResult ComparisonNe(Binary val)
        {
            return new Binary(this.Value != val.Value).SetContext(this.Context);
        }

        internal InterpreterResult Notted()
        {
            return new Binary(!this.Value).SetContext(this.Context);
        }

        public bool IsTrue()
        {
            return Value;
        }
    }
}
