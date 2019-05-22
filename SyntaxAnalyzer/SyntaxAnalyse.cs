using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SyntaxAnalyzer
{
    public class SyntaxAnalyse
    {
        private string[] grammar;
        public SyntaxAnalyse()
        {
            string buf = File.ReadAllText(@"Formal grammar.txt", Encoding.UTF8);
            grammar = buf.Split(new String[] { "\n" }, StringSplitOptions.None);
            List<State> temp = new List<State>();
            string substring = "";
            string founded;
            int skipCount = 0;
            int count = 0;
            bool repeat = false;
            for (int j = 0; j < grammar.Length; j++)
            {
                substring = grammar[j];
                founded = FindBeginning(grammar[j]);
                if (skipCount > 0)
                {
                    skipCount--;
                }
                else
                {
                    count = founded.Length + 2;
                    if (j + 1 != grammar.Length && BranchRepeat(grammar[j], grammar[j + 1]))
                        temp.Add(new State(ExpectedTerminal(substring, count), true));
                    else temp.Add(new State(ExpectedTerminal(substring, count), false));
                    repeat = false;
                }
                if (!repeat)
                {
                    while (j + 1 != grammar.Length && BranchRepeat(grammar[j], grammar[j + 1]))
                    {
                        repeat = true;
                        count = founded.Length + 2;
                        if (j + 2 != grammar.Length && BranchRepeat(grammar[j + 1], grammar[j + 2]))
                            temp.Add(new State(ExpectedTerminal(grammar[j + 1], count), true));
                        else temp.Add(new State(ExpectedTerminal(grammar[j + 1], count), false));
                        skipCount++;
                        j++;
                    }
                    j = j - skipCount;
                }
                for (int i = founded.Length + 2; i < substring.Length; i++)
                    temp.Add(new State(FindState(substring, ref i), false));
            }
            StateMachineBuilder ex = new StateMachineBuilder();
            ex.SaveToFile(temp);
            /*for (int i = 0; i < temp.Count; i++)
            {
                Console.WriteLine("Name: {0} Suppress: {1}", temp[i].Name, temp[i].Suppress);
            }
            Console.WriteLine(temp.Count);*/
        }
        private int JumpToRule(string nonterminal)
        {
            int i = 0;
            while (FindBeginning(grammar[i]) != nonterminal)
            {
                i++;
            }
            return i;
        }
        private string ExpectedTerminal(string substring, int count)
        {
            string res = "";
            res = FindState(substring, ref count);
            if (FindNonterminal(res))
            {
                JumpToRule(res);
                return null;
            }
            return res;
            
        }
        private static string FindState(string substring, ref int i)
        {
            string lex = "";
            while (substring[i] != ' ' && substring[i] != '\r' && substring[i] != '\n')
            {
                lex += substring[i];
                i++;
            }
            return lex;
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
            if (Regex.IsMatch(lexem[0].ToString(), @"[A-ZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ]")&&lexem.Length!=1)
            {
                return true;
            }
            return false;
        }
    }
}