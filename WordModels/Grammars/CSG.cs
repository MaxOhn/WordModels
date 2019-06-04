using System;
using System.Linq;
using System.Collections.Generic;
using WordModels.Utility;
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
            var allSymbols = new HashSet<string>(NTS.Union(TS));
            bool x = rules.Any(r => !allSymbols.IsSupersetOf(r.Key.Symbols));
            bool y = rules.Any(r => r.Value.Any(rs => !rs[0].Equals("") && !allSymbols.IsSupersetOf(rs.Symbols)));
            if (rules.Any(r => !allSymbols.IsSupersetOf(r.Key.Symbols) || r.Value.Any(rs => !rs[0].Equals("") && !allSymbols.IsSupersetOf(rs.Symbols))))
                throw new ArgumentException($"Rules can not contain symbols that are neither NTS nor TS!");
        }

        public bool IsContextFree() => rules.Keys.All(rs => rs.Count() == 1);

        public override string ToString()
        {
            //Utilities.PrettyPrint(rules);
            return $"({{{string.Join(", ", NTS)}}}, {TS}, {S}, {rules})";
        }
    }
}