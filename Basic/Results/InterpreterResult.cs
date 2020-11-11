namespace Basic
{
    public class InterpreterResult
    {
        public dynamic Result { get; set; }
        public CompilationError Error { get; set; }
        public InterpreterResult(dynamic result, CompilationError error)
        {
            Result = result;
            Error = error;
        }

        public override string ToString()
        {
            if (Error != null) return Error.ToString();
            return Result.ToString();
        }

        public static implicit operator InterpreterResult(Number num)
        {
            return new InterpreterResult(num, null);
        }
        public static implicit operator InterpreterResult(Binary bin)
        {
            return new InterpreterResult(bin, null);
        }
        public static implicit operator InterpreterResult(CompilationError error)
        {
            return new InterpreterResult(null, error);
        }
        
    }
}
