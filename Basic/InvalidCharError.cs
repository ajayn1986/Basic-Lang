namespace Basic
{
    public class InvalidCharError : CompilationError
    {
        public InvalidCharError(string errorDetails, Position pos_start, Position pos_end) :
            base("Invalid character", errorDetails, pos_start, pos_end)
        {

        }
    }
}
