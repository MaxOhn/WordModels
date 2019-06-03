using System.Linq;
using System.Collections.Generic;

namespace WordModels.Automata.Elements
{
    public class States : SortedSet<State>
    {
        private static int StatesComparer(State s1, State s2)
        {
            int s1Comma = s1.Name.Count(c => c.Equals(','));
            int s2Comma = s2.Name.Count(c => c.Equals(','));
            return s1Comma != s2Comma ? s1Comma - s2Comma : string.Compare(s1.Name, s2.Name);
        }

        public States() : base(Comparer<State>.Create(StatesComparer)) { }

        public States(State s) : base(Comparer<State>.Create(StatesComparer)) => Add(s);

        public States(IEnumerable<State> other) : base(other, Comparer<State>.Create(StatesComparer)) => this.OrderBy(state => state.Name);

        public override string ToString() => "{" + string.Join(", ", this) + "}";
    }
}