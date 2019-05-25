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
                int currentPos = 0;
                string buff = "";
                string swap = "";
                int length = 30;
                bool enter = false;
                sw.WriteLine("{0,-3} {1,-30} {2,-3} {3,-3} {4,-3} {5,-3} {6,-3}\n", "№", "Ожидаемый терминал", "Переход", "Принять", "В стек", "Из стека", "Ошибка");
                foreach (State state in states)
                {
                    buff = state.Name;
                    if (buff.Length<30)
                        sw.WriteLine("{0,-3} {1,-30} {2,-7} {3,-7} {4,-6} {5,-8} {6,-7} ", i, state.Name, state.Jump, state.Accept, state.ToStack,state.FromStack,state.Suppress);
                    else
                    {
                        while (currentPos < buff.Length-1)
                        {
                            if (length + currentPos > buff.Length)
                                length = buff.Length - currentPos;
                            while (buff.Length == currentPos + length||buff[length+ currentPos] != ' ')
                                length--;
                            if (!enter)
                                sw.WriteLine("{0,-3} {1,-30} {2,-7} {3,-7} {4,-6} {5,-8} {6,-7} ", i, buff.Substring(currentPos, length), state.Jump, state.Accept, state.ToStack, state.FromStack, state.Suppress);
                            else
                            {
                                swap = buff.Substring(currentPos, length);
                                if (swap[0] == ' ')
                                    swap = swap.Substring(1);
                                if(swap.Length>0&&swap!=" ")
                                    sw.WriteLine("{0,-3} {1,-30} ", " ", swap);
                            }
                            currentPos += length;
                            length = 30;
                            while (currentPos > buff.Length-1)
                                currentPos--;
                            while (buff[currentPos] != ' '|| buff.Length==currentPos)
                                currentPos--;
                            enter = true;
                        }
                        enter = false;
                        currentPos = 0;
                    }
                    i++;
                }
                sw.WriteLine();
            }
        }
    }
}
