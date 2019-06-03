using System;
using System.Linq;
using System.Collections.Generic;
using WordModels.Grammars.Elements;

namespace WordModels.Grammars
{
    public class CSG
    {
        protected HashSet<string> NTS;
        protected Alphabet TS;
        protected string S;
        protected Rules rules;
        public CSG(HashSet<string> NTS, Alphabet TS, string S, Rules rules)
        {
            this.NTS = NTS;
            this.TS = TS;
            this.S = S;
            this.rules = rules;
            if (NTS.Intersect(TS).Count() > 0)
                throw new ArgumentException("NTS and TS must be disjoint!");
            if (!NTS.Contains(S))
                throw new ArgumentException("S must be contained in NTS!");
            if (rules.Keys.Any(rs => TS.IsSupersetOf(rs.Symbols)))
                throw new ArgumentException($"Left-hand side of all rules must contain NTS!");
        }

        public bool IsContextFree() => rules.Keys.All(rs => rs.Symbols.Count() == 1);

        public override string ToString() => $"({NTS}, {TS}, {S}, {rules})";
    }
}