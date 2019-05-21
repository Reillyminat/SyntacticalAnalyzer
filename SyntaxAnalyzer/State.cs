using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalyzer
{
    class State
    {
        string rule;
        int number;
        public State(string line, int _number)
        {
            rule = line;
            number = _number;
        }
    }
}
