using System;
using System.Linq;
using System.Collections.Generic;

namespace WordModels.Grammars.Elements
{
    public class RuleSide : IEquatable<RuleSide>
    {
        public List<string> Symbols { get; set; }

        public RuleSide(string symbol) => Symbols = new List<string> { symbol };

        public RuleSide(char symbol) => Symbols = new List<string> { symbol.ToString() };

        public RuleSide(IEnumerable<string> symbols) => Symbols = new List<string>(symbols);

        public RuleSide(params string[] values) => Symbols = values.ToList();

        public void Add(string symbol)
        {
            if (Symbols.Count == 0)
                Symbols.Add(symbol);
            else
            {
                Symbols.RemoveAll(s => s.Equals(""));
                Symbols.Add(symbol);
            }
        }

        public int Count() => Symbols.Count();

        public bool Contains(string symbol) => Symbols.Contains(symbol);

        public bool Contains(IEnumerable<string> symbols)
        {
            var symEnum = symbols.GetEnumerator();
            for (int i = 0; i < Count(); i++)
            {
                if (!symEnum.MoveNext())
                    return true;
                if (!Symbols[i].Equals(symEnum.Current))
                {
                    symEnum = symbols.GetEnumerator();
                    continue;
                }
            }
            return false;
        }

        public bool Contains(RuleSide other) => Contains(other.Symbols);

        public RuleSide Except(IEnumerable<string> symbols)
        {
            var symEnum = symbols.GetEnumerator();
            for (int i = 0; i < Count(); i++)
            {
                if (!symEnum.MoveNext())
                {
                    int start = i - symbols.Count();
                    for (int j = start; j < i; j++)
                        Symbols.RemoveAt(start);
                    return this;
                }
                if (!Symbols[i].Equals(symEnum.Current))
                {
                    symEnum = symbols.GetEnumerator();
                    continue;
                }
            }
            return this;
        }

        public RuleSide Except(RuleSide other) => Except(other.Symbols);

        public RuleSide Except(params string[] symbols) => Except(symbols);

        public int RemoveAll(string symbol) => Symbols.RemoveAll(s => s.Equals(symbol));

        public void RemoveAt(int idx) => Symbols.RemoveAt(idx);

        public int Replace(string symbol, string replacement)
        {
            int idx;
            int count = 0;
            while ((idx = Symbols.FindIndex(s => s.Equals(symbol))) != -1)
            {
                Symbols[idx] = replacement;
                count++;
            }
            return count;
        }

        public void ReplaceFirstNWidth(int n, string replacement)
        {
            if (n < 0 || n >= Symbols.Count())
                throw new ArgumentOutOfRangeException("RuleSide does not contain sufficiently many elements to replace!");
            for (int i = 0; i < n; i++)
                Symbols.RemoveAt(0);
            Symbols.RemoveAll(s => s.Equals(""));
            Symbols.Insert(0, replacement);
        }

        public RuleSide GetReplace(string symbol, string replacement)
        {
            RuleSide nSide = new RuleSide();
            foreach (string s in Symbols)
                nSide.Add(s.Equals(symbol) ? replacement : s);
            return nSide;
        }

        public string this[int idx] => Symbols[idx];

        public override string ToString()
        {
            if (Symbols.Count() == 0)
                throw new InvalidOperationException("RuleSide may not be empty!");
            return string.Join("", Symbols.Select(s => s.Equals("") ? "eps" : s));
        }

        public bool Equals(RuleSide other)
        {
            if (!(other is RuleSide item))
                return false;
            for (int i = 0; i < Symbols.Count(); i++)
                if (!this[i].Equals(item[i]))
                    return false;
            return true;
        }

        public bool Equals(string symbol) => Symbols.Count() == 1 && Symbols[0].Equals(symbol);

        public bool Equals(char symbol) => Symbols.Count() == 1 && Symbols[0].Equals(symbol.ToString());

        public override int GetHashCode() => string.Join("", Symbols).GetHashCode();
    }
}
