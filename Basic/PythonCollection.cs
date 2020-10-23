using System.Collections;
using System.Collections.Generic;

namespace Basic
{
    public class PythonCollection<T> : IEnumerable
    {
        public List<T> Tokens { get; set; }
        public PythonCollection()
        {
            Tokens = new List<T>();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (T tok in Tokens)
                yield return tok;
        }

        public void Append(T tok) => Tokens.Add(tok);

        public override string ToString()
        {
            string repr = "";
            foreach (T tok in Tokens)
            {
                if (!string.IsNullOrWhiteSpace(repr))
                    repr += ", ";
                repr += tok.ToString();
            }
            return repr;
        }

        public T this[int index]
        {
            get { return Tokens[index]; }
        }

        public int Length { get => Tokens.Count; }
        
    }
}
