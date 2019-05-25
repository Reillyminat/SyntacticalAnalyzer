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
        private List<string>[] str;
        public SyntaxAnalyse()
        {
            string buf = File.ReadAllText(@"Formal grammar.txt", Encoding.UTF8);
            grammar = buf.Split(new String[] { "\n" }, StringSplitOptions.None);
            List<State> temp = new List<State>();
            List<string> heap = new List<string>();
            str = new List<string>[grammar.Length];
            string substring = "";
            string buff = "";
            string founded;
            int skipCount = 0;
            int k = 0;
            bool repeat = false;
            bool terminal = true;
            Transform();
            for (int j = 0; j < str.Length; j++)
            {
                substring = grammar[j];
                founded = str[j][0];
                if (skipCount > 0)
                {
                    skipCount--;
                }
                else
                {
                    if (j + 1 != str.Length && BranchRepeat(str[j], str[j + 1]))
                    {
                        buff = ExpectedTerminal(str[j][k], ref terminal, j,ref heap, str[j][k],k);
                        if (terminal)
                            temp.Add(new State(str[j][k] + " " + buff, terminal, -1, true));
                        else
                            temp.Add(new State(str[j][k] + " " + buff, terminal, temp.Count + 1, true));
                        terminal = true;
                    }
                    else
                    {
                        buff = ExpectedTerminal(str[j][k], ref terminal, j,ref heap, str[j][k],k);
                        if (terminal)
                            temp.Add(new State(str[j][k] + " " + buff, terminal, -1, false));
                        else
                            temp.Add(new State(str[j][k] + " " + buff, terminal, temp.Count + 1, false));
                        terminal = true;
                    }
                    repeat = false;
                }
                if (!repeat)
                {
                    while (j + 1 != str.Length && BranchRepeat(str[j], str[j + 1]))
                    {
                        repeat = true;
                        if (j + 2 != str.Length && BranchRepeat(str[j + 1], str[j + 2]))
                        {
                            buff = ExpectedTerminal(str[j][k], ref terminal, j + 1,ref heap, str[j][k],k);
                            if (terminal)
                                temp.Add(new State(str[j][k]+" "+buff, terminal, -1, true));
                            else
                                temp.Add(new State(str[j][k] + " " + buff, terminal, temp.Count + 1, true));
                            terminal = true;
                        }
                        else
                        {
                            buff = ExpectedTerminal(str[j][k], ref terminal, j + 1,ref heap, str[j][k],k);
                            if (terminal)
                                temp.Add(new State(str[j][k] + " " + buff, terminal, -1, false));
                            else
                                temp.Add(new State(str[j][k] + " " + buff, terminal, temp.Count + 1, false));
                            terminal = true;
                        }
                        skipCount++;
                        j++;
                    }
                    j = j - skipCount;
                }
                for (int i = 1; i < str[j].Count; i++)
                {
                    buff = ExpectedTerminal(str[j][i], ref terminal, j,ref heap, str[j][i],i);
                    if (terminal)
                        temp.Add(new State(buff, terminal, -1, false));
                    else
                        temp.Add(new State(str[j][i] + " " + buff, terminal, temp.Count + 1, false));
                    terminal = true;
                }
                k = 0;
            }
            StateMachineBuilder ex = new StateMachineBuilder();
            ex.SaveToFile(temp);
            for (int i = 0; i < temp.Count; i++)
            {
                Console.WriteLine("Name: {0} Suppress: {1}", temp[i].Name, temp[i].Suppress);
            }
            Console.WriteLine(temp.Count);
        }
        private int JumpToRule(string nonterminal)
        {
            int i = 0;
            if (nonterminal.Contains(" "))
                nonterminal=nonterminal.Remove(nonterminal.Length - 1, 1);
            while (str[i][0] != nonterminal)
            {
                i++;
            }
            return i;
        }
        private string ExpectedTerminal(string word, ref bool terminal, int jump, ref List<string> heap, string nonterm, int offset)
        {
            string temp = "";
            bool alt = false;
            if (FindNonterminal(word))
            {
                terminal = false;
                do
                {
                    if (word == "EM")
                    {
                        word = "";
                        return word;
                    }
                    if (!Existance(heap, nonterm))
                    {
                        heap.Add(word);
                        /*if (offset + 1 == str[jump].Count)
                            return temp;*/
                        word = ExpectedTerminal(str[jump][1], ref terminal, jump, ref heap, str[jump][1],offset);
                        if (!FindNonterminal(word))
                        {
                            temp += word + ' ';
                            break;
                        }
                    }
                    if (word != "" && !alt)
                        jump = JumpToRule(word);
                    alt = true;
                    word = str[jump][1];
                    if (FindNonterminal(word)&& !BranchRepeat(str[jump], str[jump+1]))
                    {
                        jump = JumpToRule(word);
                        word = ExpectedTerminal(str[jump][1], ref terminal, jump, ref heap, str[jump][1], offset);
                    }
                    temp += word + ' ';
                    jump++;
                } while (jump + 1 != str.Length && BranchRepeat(str[jump-1], str[jump]) && alt);
                heap.RemoveRange(0,heap.Count);
                return temp;
            }
            return word;
        }
        private bool Existance(List<string> heap,string name)
        {
            string found = heap.Find(x => x == name);
            if (found == null)
                return false;
            else return true;
        }
        private static string FindState(string substring, ref int i)
        {
            string lex = "";
            if (substring[i] == ' ')
                i++;
            while (substring[i] != ' ' && substring[i] != '\r' && substring[i] != '\n')
            {
                if (substring[i] == '-' && substring[i + 1] == '>' && i < substring.Length - 1)
                {
                    i += 2;
                    return lex;
                }
                else
                {
                    lex += substring[i];
                    i++;
                }
            }
            return lex;
        }
        private void Transform()
        {
            int count = 0;
            string temp = "";
            for (int i = 0; i < str.Length; i++)
            {
                str[i] = new List<string>();
                while (count != grammar[i].Length - 1)
                {
                    temp = FindState(grammar[i], ref count);
                    str[i].Add(temp);
                }
                count = 0;
            }
        }
        private static bool BranchRepeat(List<string> first, List<string> second)
        {
            if (first[0] == second[0])
                return true;
            else return false;
        }
        private static bool FindNonterminal(string lexem)
        {
            if (lexem != "" && Regex.IsMatch(lexem[0].ToString(), @"[A-ZАБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ]") && lexem.Length != 1 && !lexem.Contains(" ")&&lexem!="EM")
            {
                return true;
            }
            return false;
        }
    }
}