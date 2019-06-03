using System;
using System.Linq;
using System.Collections.Generic;

namespace WordModels.Grammars.Elements
{
    public class RuleSide : IEquatable<RuleSide>
    {
        public List<string> Symbols { get; set; }

        public RuleSide(string symbol) => Symbols = new List<string> { symbol };

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

        public bool Contains(RuleSide other) => other.Except(Symbols).Count() == 0;

        public RuleSide Except(IEnumerable<string> symbols) => new RuleSide(Symbols.Except(symbols));

        public RuleSide Except(RuleSide other) => new RuleSide(Symbols.Except(other.Symbols));

        public int RemoveAll(string symbol) => Symbols.RemoveAll(s => s.Equals(symbol));

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

        public string this[int idx] => Symbols[idx];

        public override string ToString() => string.Join("", Symbols.Select(s => s.Equals("") ? "eps" : s));

        public bool Equals(RuleSide other)
        {
            if (!(other is RuleSide item))
                return false;
            for (int i = 0; i < Symbols.Count(); i++)
                if (!this[i].Equals(item[i]))
                    return false;
            return true;
        }

        public override int GetHashCode() => string.Join("", Symbols).GetHashCode();
    }
}
