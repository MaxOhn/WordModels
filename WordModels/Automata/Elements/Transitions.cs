using System;
using System.Collections.Generic;
using System.Linq;

namespace WordModels.Automata.Elements
{
    public class Transitions : Dictionary<(State Source, string Letter), States>
    {
        public void Add(State q, string letter, State p)
        {
            if (ContainsKey((q, letter)))
                this[(q, letter)].Add(p);
            else
                Add((q, letter), new States(p));
        }

        public void Remove(State q, string letter, State p)
        {
            if (ContainsKey((q, letter)))
                this[(q, letter)].Remove(p);
        }

        public override string ToString() => "{" + string.Join(", ", this.Select(kvp => $"{kvp.Key.Source} -{kvp.Key.Letter}-> {kvp.Value}")) + "}";
    }
}
