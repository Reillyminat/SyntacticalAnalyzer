using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SyntaxAnalyzer
{
    public class SyntaxAnalyse
    {
        State[] SA;
        public SyntaxAnalyse()
        {
            string buf = File.ReadAllText(@"BNF.txt", Encoding.Default);
            string[] grammar = buf.Split(new String[] { "\n" }, StringSplitOptions.None);
            List <string> temp = new List<string>();
            string lex="";
            foreach(char t in buf)
            {
                if (t == ' ' || t == '\n')
                {
                    temp.Add(lex);
                    lex = "";
                }
                else lex += t;   
            }
            SA = new State[grammar.Length];
            bool repeat=false;
            for (int i = 0; i < grammar.Length; i++)
            {
                if (grammar.Length != i - 1)
                {
                    if (BranchRepeat(grammar[i], grammar[i + 1]))
                        repeat = true;
                    else repeat = false;
                    SA[i] = new State(grammar[i],);
                }
            }
        }
        public void Parse()
        {

        }
        public static string FindBeginning(string line)
        {
            string begining = "";
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '-')
                    break;
                begining += line[i];
            }
            return begining;
        }
        public static bool BranchRepeat(string first, string second)
        {
            if (FindBeginning(first) == FindBeginning(second))
                return true;
            else return false;
        }
    }
}