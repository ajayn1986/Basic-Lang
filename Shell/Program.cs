using Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string code = Input("Basic > ");
                var result = Basic.Basic.Run(code, "Console");
                Console.WriteLine(Convert.ToString(result));
            }
        }

        static string Input(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
