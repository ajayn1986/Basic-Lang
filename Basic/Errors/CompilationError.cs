namespace Basic
{
    public class CompilationError
    {
        public CompilationError(string errorName, string errorDetails, Position pos_start, Position pos_end)
        {
            ErrorName = errorName;
            ErrorDetails = errorDetails;
            Pos_Start = pos_start;
            Pos_End = pos_end;
        }

        public override string ToString()
        {
            return $"{ErrorName} : {ErrorDetails}\nFile : {Pos_Start.Fn}, Line : {Pos_Start.Ln + 1 }, Col : {Pos_Start.Col + 1}"
                + "\n\n" + StringWithArrows.Get(Pos_Start.Text, Pos_Start, Pos_End);
        }

        public string ErrorName { get; private set; }
        public string ErrorDetails { get; private set; }
        public Position Pos_Start { get; private set; }
        public Position Pos_End { get; private set; }
    }
}
