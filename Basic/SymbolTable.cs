using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic
{
    public class SymbolTable
    {
        Hashtable symblols;
        public SymbolTable Parent { get; set; }
        public SymbolTable()
        {
            symblols = new Hashtable();
            Parent = null;
        }

        public dynamic this[string name]
        {
            get {
                dynamic value = symblols.ContainsKey(name) ? symblols[name] : null;
                if (value == null && Parent != null)
                    value = Parent[name];
                return value;
            }
            set {
                symblols[name] = value;
            }
        }

        public void Remove(string name)
        {
            symblols.Remove(name);
        }
    }
}
