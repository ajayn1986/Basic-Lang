namespace Basic
{
    public class RuntimeError : CompilationError
    {
        public Context Context { get; set; }
        public RuntimeError(string errorDetails, Position pos_start, Position pos_end, Context context)
            : base("Runtime Error", errorDetails, pos_start, pos_end)
        {
            Context = context;
        }

        public override string ToString()
        {
            var result = GenerateTraceback();
            result += $"{this.ErrorName} : {this.ErrorDetails}";
            result += "\n\n" + StringWithArrows.Get(this.Pos_Start.Text, this.Pos_Start, this.Pos_End);
            return result;
        }

        private string GenerateTraceback()
        {
            string result = "";
            var pos = this.Pos_Start;
            var ctx = this.Context;

            while (ctx != null)
            {
                result = $" File {pos.Fn}, line {(pos.Ln + 1)}, in {ctx.DisplayName} \n" + result;
                pos = ctx.Parent_Entry_Pos;
                ctx = ctx.Parent;
            }
            return "Traceback (most recent call last) : \n" + result;
        }
    }
}
