using System;
using System.Linq;
using System.Collections.Generic;
using WordModels.Grammars.Elements;

namespace WordModels.Grammars
{
    public class CFG : CSG
    {
        public CFG(HashSet<string> NTS, Alphabet TS, string S, Rules rules)
        : base(NTS, TS, S, rules)
        {
            if (!IsContextFree())
                throw new ArgumentException("Left-hand side of rules may only contain one NTS!");
        }

        public bool IsInCNF()
        {
            return rules.All(r =>
            {
                return r.Value.All(rs =>
                {
                    switch (rs.Count())
                    {
                        case 1: return TS.IsSupersetOf(rs.Symbols);
                        case 2: return NTS.IsSupersetOf(rs.Symbols);
                        default: return false;
                    }
                }) && (r.Key.Symbols[0].Equals(S) || r.Value.All(rs => !rs.Contains("")));
            });
        }

        public CFG GetCNF()
        {
            HashSet<string> nNTS = NTS;
            Alphabet nTS = TS;
            string nS = S;
            Rules nRules = rules;

            // Phase 0

            var emptyableNTS = rules.Where(r => r.Value.Any(rs => rs.Contains(""))).Select(r => r.Key);
            if (emptyableNTS.Count() > 0)
            {
                foreach (RuleSide rs in emptyableNTS)
                    nRules[rs] = new HashSet<RuleSide>(nRules[rs].Where(side => !side.Contains("")));
                Queue<RuleSide> queue = new Queue<RuleSide>(emptyableNTS);
                while (queue.Count() > 0)
                {
                    RuleSide curr = queue.Dequeue();
                    var impacted = nRules.Contains(curr, false);
                    foreach (RuleSide rs in impacted)
                    {
                        queue.Enqueue(rs);
                        nRules.CopyRightSideWithout(rs, curr);
                    }
                }
                nS = S + "'";
                nRules.Add(new RuleSide(nS), new RuleSide(S));
                nRules.Add(new RuleSide(nS), new RuleSide(""));
            }

            // Phase 1

            // TODO

            // Phase 2

            // TODO

            // Phase 3

            // TODO

            return this;
        }
        public bool IsInLanguage(string word)
        {
            if (!IsInCNF())
                throw new InvalidOperationException("Can only check language inclusion for grammars in CNF!");

            // TODO

            return false;
        }
    }
}