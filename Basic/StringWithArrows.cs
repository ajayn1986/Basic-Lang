using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class StringWithArrows
    {
        public static string Get(string text, Position pos_start, Position pos_end)
        {
            var result = "";

            //Calculate indices
            var idx_start = Math.Max(text.LastIndexOf('\n', Math.Min(text.Length - 1, pos_start.Idx), Math.Min(text.Length - 1, pos_start.Idx)), 0);
            var idx_end = text.IndexOf('\n', idx_start + 1);
            if (idx_end < 0) idx_end = text.Length;

            //Generate each line
            var line_count = pos_end.Ln - pos_start.Ln + 1;
            foreach (int i in Enumerable.Range(0, line_count))
            {
                //Calculate line columns
                var line = text.Substring(idx_start, idx_end - idx_start);
                var col_start = (i == 0) ? pos_start.Col : 0;
                var col_end = i == line_count - 1 ? pos_end.Col : line.Length - 1;


                //Append to result
                result += line + '\n';
                string start = string.Empty;
                start = start.PadLeft(col_start, ' ');
                result += start.PadRight(col_end, '^');

                //Re-calculate indices
                idx_start = idx_end;
                if (idx_start >= text.Length) idx_start = text.Length - 1;
                idx_end = text.IndexOf('\n', idx_start + 1);
                if (idx_end < 0) idx_end = text.Length;
            }

            return result.Replace("\t", string.Empty);
        }
    }
}
