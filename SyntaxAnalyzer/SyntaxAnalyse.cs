using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SyntaxAnalyzer
{
    public class SyntaxAnalyse
    {
        public SyntaxAnalyse()
        {
            string buf = File.ReadAllText(@"Formal grammar.txt", Encoding.UTF8);
            string[] grammar = buf.Split(new String[] { "\n" }, StringSplitOptions.None);
            List<State> temp = new List<State>();
            string lex = "";
            string substring = "";
            string founded;
            int skipCount = 0;
            bool repeat = false;
            bool nonterminal = false;
            for (int j = 0; j < grammar.Length; j++)
            {
                substring = grammar[j];
                founded = FindBeginning(grammar[j]);
                nonterminal=FindNonterminal(founded);
                if (skipCount > 0)
                {
                    skipCount--;
                }
                else
                {
                    if (j + 1 != grammar.Length && BranchRepeat(grammar[j], grammar[j + 1]))
                        temp.Add(new State(founded, true));
                    else temp.Add(new State(founded, false));
                    repeat = false;
                }
                if (!repeat)
                {
                    while (j + 1 != grammar.Length && BranchRepeat(grammar[j], grammar[j + 1]))
                    {
                        repeat = true;
                        if (j + 2 != grammar.Length && BranchRepeat(grammar[j + 1], grammar[j + 2]))
                            temp.Add(new State(FindBeginning(grammar[j + 1]), true));
                        else temp.Add(new State(FindBeginning(grammar[j + 1]), false));
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
                            temp.Add(new State(lex, false));
                            lex = "";
                            continue;
                        }
                        if (substring[i + 1] == '>')
                        {
                            temp.Add(new State(lex, false));
                            lex = "";
                            lex += substring[i + 1];
                            i++;
                        }
                        else
                        {
                            temp.Add(new State(lex, false));
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

            }
            StateMachineBuilder ex = new StateMachineBuilder();
            ex.SaveToFile(temp);
            for (int i = 0; i < temp.Count; i++)
            {
                Console.WriteLine("Name: {0} Suppress: {1}", temp[i].Name, temp[i].Suppress);
            }
            Console.WriteLine(temp.Count);
        }
        private static string FindBeginning(string line)
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
        private static bool BranchRepeat(string first, string second)
        {
            if (FindBeginning(first) == FindBeginning(second))
                return true;
            else return false;
        }
        private static bool FindNonterminal(string lexem)
        {
            if (Regex.IsMatch(lexem[0].ToString(), @"[A-ZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ]"))
            {
                return true;
            }
            return false;
        }
    }
}