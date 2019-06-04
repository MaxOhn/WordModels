using System.Collections.Generic;
using System.Linq;

namespace WordModels.Grammars.Elements
{
    public class Rules : Dictionary<RuleSide, HashSet<RuleSide>>
    {
        public void Add(RuleSide left, RuleSide right)
        {
            if (ContainsKey(left))
                this[left].Add(right);
            else
                Add(left, new HashSet<RuleSide> { right });
        }

        public void AddToKey(RuleSide left, IEnumerable<RuleSide> rights) => this[left].UnionWith(rights);

        public void Remove(RuleSide left, RuleSide right)
        {
            if (ContainsKey(left))
            {
                this[left].Remove(right);
                if (this[left].Count() == 0)
                    Remove(left);
            }
        }

        public void Replace(RuleSide left, RuleSide right, RuleSide replacement)
        {
            if (ContainsKey(left) && this[left].Contains(right))
            {
                this[left].Remove(right);
                this[left].Add(replacement);
            }
        }

        public void CopyRightSideWithout(RuleSide left, RuleSide without)
        {
            if (ContainsKey(left))
            {
                foreach (RuleSide rs in this[left].ToList())
                {
                    if (rs.Contains(without))
                    {
                        RuleSide nSide = rs.Except(without);
                        if (nSide.Count() > 0)
                            this[left].Add(rs.Except(without));
                    }
                }
            }
        }

        public bool Contains(RuleSide left, RuleSide right, bool single)
        {
            if (!ContainsKey(left))
                return false;
            return single ? this[left].Contains(right) : this[left].Any(rs => rs.Contains(right));
        }

        public IEnumerable<RuleSide> Contains(RuleSide right, bool single) => this.Where(r => Contains(r.Key, right, single)).Select(r => r.Key);

        public override string ToString() => $"{{{string.Join(", ", this.Select(r => $"{r.Key} -> {string.Join(" | ", r.Value)}"))}}}";
    }
}
