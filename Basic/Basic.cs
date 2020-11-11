using System;

namespace Basic
{
    public class Basic
    {
        public static SymbolTable globalSymbolTable = new SymbolTable();
        public static object Run(string text, string filename)
        {
            globalSymbolTable["null"] = new Null();
            globalSymbolTable["true"] = new Binary(true);
            globalSymbolTable["false"] = new Binary(false);
            Lexer lexer = new Lexer(text, filename);
            var lexerResult = lexer.MakeTokens();
            if (lexerResult.Item2 != null) return lexerResult.Item2;
            Parser parser = new Parser(lexerResult.Item1);
            var parseResult = parser.Parse();
            if (parseResult.Error != null) return parseResult.Error;
            Interpreter interpreter = new Interpreter();
            var interpreterRes = interpreter.Visit(parseResult.Node, new Context("<Program>") { SymbolTable = globalSymbolTable });
            if (interpreterRes.Error != null) return interpreterRes.Error;
            return interpreterRes.Result;
        }
    }
}
