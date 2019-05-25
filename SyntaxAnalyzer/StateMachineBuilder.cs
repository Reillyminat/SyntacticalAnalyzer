using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyntaxAnalyzer
{
    class StateMachineBuilder
    {
        public void SaveToFile(List<State> states)
        {
            using (FileStream fs = new FileStream(@"State machine.txt", FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                int i = 0;
                int subString = 0;
                sw.WriteLine("{0,-3} {1,-30} {2,-3} {3,-3} {4,-3} {5,-3} {6,-3}\n", "№", "Ожидаемый терминал", "Переход", "Принять", "В стек", "Из стека", "Ошибка");
                foreach (State state in states)
                {
                    i++;
                    if(state.Name.Count()<30)
                        sw.WriteLine("{0,-3} {1,-30} {2,-7} {3,-7} {4,-6} {5,-8} {6,-7} ", i, state.Name, i, state.Accept, state.ToStack,i,state.Suppress);
                    else
                    {
                        while (subString < state.Name.Count())
                        {
                            sw.WriteLine("{0,-3} {1,-30} ", " ", state.Name.Substring(subString));
                            subString += 15;
                        }
                    }
                }
                sw.WriteLine();
            }
        }
    }
}
