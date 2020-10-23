using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class Context
    {
        public Context(string displayName, Context parent = null, Position parent_entry_pos = null)
        {
            this.DisplayName = displayName;
            this.Parent = parent;
            this.Parent_Entry_Pos = parent_entry_pos;
            this.SymbolTable = new SymbolTable();
        }

        public string DisplayName { get; set; }
        public Context Parent { get; set; }
        public Position Parent_Entry_Pos { get; set; }
        public SymbolTable SymbolTable { get; set; }
    }
}
