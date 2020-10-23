using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Number
    {
        dynamic Value;
        public Position Start_pos { get; set; } = null;
        public Position End_pos { get; set; } = null;
        public Context Context { get; set; }
        public Number(dynamic value)
        {
            Value = value;
            this.SetContext();
        }
        public Number SetPos(Position start_pos, Position end_pos)
        {
            Start_pos = start_pos;
            End_pos = end_pos;
            return this;
        }
        public InterpreterResult Added_To(Number num)
        {
            return new Number(Value + num.Value).SetContext(this.Context);
        }
        public InterpreterResult Subbed_by(Number num)
        {
            return new Number(Value - num.Value).SetContext(this.Context);
        }
        public InterpreterResult Multed_by(Number num)
        {
            return new Number(Value * num.Value).SetContext(this.Context);
        }
        public InterpreterResult Dived_by(Number num)
        {
            if (num.Value == 0) return new RuntimeError("Divided by zero", num.Start_pos, num.End_pos, this.Context);
            if (num.Value is int && Value % num.Value != 0) return new Number(Value / (double)num.Value);
            return new Number(Value / num.Value).SetContext(this.Context);
        }
        public InterpreterResult Powed_by(Number num)
        {
            return new Number(Math.Pow(Value, num.Value)).SetContext(this.Context);
        }

        public Number Copy()
        {
            Number copy = new Number(this.Value);
            copy.SetPos(this.Start_pos, this.End_pos);
            copy.SetContext(this.Context);
            return copy;

        }
        public override string ToString()
        {
            return Value?.ToString();
        }
        public Number SetContext(Context context = null)
        {
            this.Context = context;
            return this;
        }
    }
}
