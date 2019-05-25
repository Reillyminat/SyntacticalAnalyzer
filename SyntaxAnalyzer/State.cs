using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyzer
{
    class State
    {
        public State(string name, bool accept, int toStack, bool suppress)
        {
            Console.WriteLine(name);
            Name = name;
            Accept = accept;
            ToStack = toStack;
            Suppress = suppress;
        }
        public string Name { get; set; }
        public int Jump { get; set; }
        public bool Accept { get; set; }
        public int ToStack{ get; set;}
        public bool FromStack { get; set; }
        public bool Suppress { get; set; }
    }
}
