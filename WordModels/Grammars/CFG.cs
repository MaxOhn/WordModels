using System;
using System.Linq;
using WordModels.Utility;
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

        public void CNF()
        {
            // Phase 0

            var emptyableNTS = rules.Where(r => r.Value.Any(rs => rs.Contains(""))).Select(r => r.Key).ToList();
            if (emptyableNTS.Count() > 0)
            {
                foreach (RuleSide rs in emptyableNTS)
                {
                    rules.Remove(rs, new RuleSide(""));
                    rules.CopyRightSideWithout(rs, rs);
                }
                Queue<RuleSide> queue = new Queue<RuleSide>(emptyableNTS);
                while (queue.Count() > 0)
                {
                    RuleSide curr = queue.Dequeue();
                    var impacted = rules.Contains(curr, false).ToList();
                    foreach (RuleSide rs in impacted)
                    {
                        rules.CopyRightSideWithout(rs, curr);
                        if (rules[rs].Contains(curr))
                        {
                            queue.Enqueue(rs);
                            rules.CopyRightSideWithout(rs, rs);
                        }
                    }
                }
                RuleSide nS = new RuleSide(S + "'");
                NTS.Add(nS.ToString());
                rules.Add(nS, new RuleSide(S));
                rules.Add(nS, new RuleSide(""));
                S = nS.ToString();
            }

            // Phase 1

            TS.ToList().ForEach(ts =>
            {
                NTS.Add("X_" + ts);
                rules.Add(new RuleSide("X_" + ts), new RuleSide(ts));
            });
            foreach (RuleSide left in rules.Keys)
            {
                foreach (RuleSide right in rules[left].ToList())
                {
                    RuleSide nSide = right;
                    foreach (string ts in TS)
                        if (right.Contains(ts) && right.Count() > 1)
                            nSide.Replace(ts, "X_" + ts);
                    rules.Replace(left, right, nSide);
                }
            }

            // Phase 2

            var transNTS = rules.Where(r => r.Value.Any(rs => rs.Count() == 1 && NTS.Contains(rs[0]))).Select(r => r.Key).ToList();
            if (transNTS.Count > 0)
            {
                Queue<RuleSide> queue = new Queue<RuleSide>(transNTS);
                while (queue.Count > 0)
                {
                    RuleSide curr = queue.Dequeue();
                    var singles = rules[curr].Where(rs => rs.Count() == 1 && NTS.Contains(rs[0])).ToList();
                    foreach (RuleSide rs in singles)
                    {
                        rules.Remove(curr, rs);
                        if (queue.Contains(rs))
                            queue.Enqueue(curr);
                        if (rules.ContainsKey(rs))
                            rules.AddToKey(curr, rules[rs]);
                    }
                }
            }

            // Phase 3

            var tooMany = rules.SelectMany(r => r.Value).Where(rs => rs.Count() > 2);
            if (tooMany.Count() > 0)
            {
                int idx = 1;
                var queue = new Queue<RuleSide>(tooMany);
                while (queue.Count > 0)
                {
                    var curr = queue.Dequeue();
                    while (curr.Count() > 2)
                    {
                        string nNTS = "Y_" + idx++;
                        RuleSide nSide = new RuleSide(curr[0], curr[1]);
                        NTS.Add(nNTS);
                        rules.Add(new RuleSide(nNTS), nSide);
                        curr.ReplaceFirstNWidth(2, nNTS);
                        var occurences = rules.Select(r => (r.Key, r.Value.Where(rs => rs.Contains(nSide)))).Where(pair => pair.Item2.Count() > 0);
                        foreach (var pair in occurences)
                            foreach (var right in pair.Item2)
                                rules.Replace(pair.Key, right, right.Except(nSide));
                    }
                }

            }
        }

        public bool ContainsWord(string word)
        {
            if (!IsInCNF())
                throw new InvalidOperationException("Can only check language inclusion for grammars in CNF!");
            int len = word.Length;
            HashSet<string>[][] table = new HashSet<string>[len][];
            for (int i = 0; i < len; i++)
            {
                table[i] = new HashSet<string>[i + 1];
                table[i][i] = new HashSet<string>(rules.Where(r => r.Value.Any(rs => rs[0].Equals(word[i].ToString()))).Select(r => r.Key.ToString()));
            }
            for (int i = 1; i < len; i++)
            {
                for (int j = 0; j < len - i; j++)
                {
                    table[j + i][j] = new HashSet<string>();
                    for (int k = j; k < i + j; k++)
                        foreach (string s1 in table[k][j])
                            foreach (string s2 in table[i + j][k + 1])
                                table[j + i][j].UnionWith(rules.Where(r => r.Value.Any(rs => rs.ToString().Equals(s1 + s2))).Select(r => r.Key.ToString()));
                }
            }
            //Utilities.PrettyPrint(table);
            return table[len-1][0].Contains(S);
        }
    }
}