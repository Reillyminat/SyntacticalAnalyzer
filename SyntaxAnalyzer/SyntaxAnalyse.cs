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
        public SyntaxAnalyse()
        {
            string buf = File.ReadAllText(@"Formal grammar.txt", Encoding.UTF8);
            string[] grammar = buf.Split(new String[] { "\n" }, StringSplitOptions.None);
            List <string> temp = new List<string>();
            string lex="";
            string substring = "";
            string founded;
            int skipCount = 0;
            bool repeat = false;
            for (int j=0;j<grammar.Length;j++)
            {
                substring = grammar[j];
                founded = FindBeginning(grammar[j]);
                if (skipCount > 0)
                {
                    skipCount--;
                }
                else {
                    temp.Add(founded);
                    repeat = false;
                }
                if (!repeat)
                {
                    while (j + 1 != grammar.Length && BranchRepeat(grammar[j], grammar[j + 1]))
                    {
                        repeat = true;
                        temp.Add(FindBeginning(grammar[j + 1]));
                        skipCount++;
                        j++;
                    }
                    j = j - skipCount;
                }
                for (int i = founded.Length; i < substring.Length; i++)
                {
                    if (substring[i] == ' ' || substring[i] == '\r' || substring[i] == '\n')
                    {
                        if (i + 1 == substring.Length)
                        {
                            temp.Add(lex);
                            lex = "";
                            continue;
                        }
                        if (substring[i + 1] == '>')
                        {
                            temp.Add(lex);
                            lex = "";
                            lex += substring[i + 1];
                            i++;
                        }
                        else
                        {
                            temp.Add(lex);
                            lex = "";
                        }
                    }
                    else if (substring[i] == '-' && substring[i + 1] == '>')
                    {
                        i++;
                        continue;
                    }
                    else lex += substring[i];
                }
            }
            for (int i = 0; i < temp.Count; i++)
            {
                Console.WriteLine(temp[i]);
            }
            Console.WriteLine(temp.Count);
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