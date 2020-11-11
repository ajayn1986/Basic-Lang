namespace Basic
{
    public class ExpectedCharError : CompilationError
    {
        public ExpectedCharError(string errorDetails, Position pos_start, Position pos_end) :
            base("Expected character", errorDetails, pos_start, pos_end)
        {

        }
    }
}
