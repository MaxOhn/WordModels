using System;
using System.Linq;
using WordModels.Automata.Elements;

namespace WordModels.Automata
{
    public class DFA : NFA
    {
        public DFA(States states, Alphabet sigma, State initialState, Transitions transitions)
            : base(states, sigma, initialState, transitions)
        {
            if (!IsDeterministic())
                throw new ArgumentException("Transitions must be deterministic!");
        }

        public void Complement() => states.ForEach(s => s.IsFinal = !s.IsFinal);

        public DFA GetComplement() => new DFA(new States(states.Select(s => new State(s.ToString(), !s.IsFinal))), sigma, initialState, transitions);
    }
}