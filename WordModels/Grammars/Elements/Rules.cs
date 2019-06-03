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

        public override string ToString() => $"{{{string.Join(", ", this.Select(rule => $"{rule.Key} -> {string.Join(" | ", rule.Value)}"))}}}";
    }
}
