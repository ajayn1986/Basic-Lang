namespace Basic
{
    public class ParseResult
    {
        public Node Node { get; set; }
        public CompilationError Error { get; set; }
        public ParseResult(Node node, CompilationError error)
        {
            Node = node;
            Error = error;
        }

        public override string ToString()
        {
            if (Error != null) return Error.ToString();
            return Node.ToString();
        }

        public bool HasError()
        {
            return Error != null;
        }

        public static implicit operator ParseResult(CompilationError error)
        {
            return new ParseResult(null, error);
        }
        public static implicit operator ParseResult(Node node)
        {
            return new ParseResult(node, null);
        }
    }
}
