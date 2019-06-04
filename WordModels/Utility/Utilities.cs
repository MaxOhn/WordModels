using System;
using System.Linq;
using System.Collections.Generic;
using WordModels.Grammars.Elements;

namespace WordModels.Utility
{
    class Utilities
    {
        public static void PrettyPrint(IEnumerable<object>[][] table)
        {
            int len = table.Length;
            var colWidth = Enumerable.Repeat(0, len).ToList();
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < len; j++)
                {
                    if (i > j)
                        continue;
                    colWidth[j] = Math.Max(colWidth[j], $"{{{string.Join(", ", table[j][i])}}}".Length + 3);
                }
            }
            string print = " +" + string.Join("+", colWidth.Select(num => new string('-', num - 1))) + "+\n";
            for (int i = 0; i < len; i++)
            {
                string floor = " ";
                for (int j = 0; j < len; j++)
                {
                    if (i > j)
                    {
                        print += new string(' ', colWidth[j]);
                        floor += new string(' ', colWidth[j]);
                    }
                    else
                    {
                        string nS = $" | {{{string.Join(", ", table[j][i])}}}";
                        print += nS + new string(' ', colWidth[j] - nS.Length);
                        floor += "+" + new string('-', colWidth[j] - 1);
                    }
                }
                print += " |\n";
                print += floor + "+\n";
            }
            Console.WriteLine(print);
        }

        public static void PrettyPrint(Rules rules) => Console.WriteLine(string.Join("\n", rules.Select(r => $"{r.Key} -> {string.Join(" | ", r.Value)}")));
    }
}
