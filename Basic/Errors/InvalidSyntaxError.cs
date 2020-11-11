namespace Basic
{
    public class InvalidSyntaxError : CompilationError
    {
        public InvalidSyntaxError(string errorDetails, Position pos_start, Position pos_end) : base("Invalid syntax", errorDetails, pos_start, pos_end)
        {

        }
    }
}
