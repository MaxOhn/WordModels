using System;
using System.Collections.Generic;
using System.Linq;
using WordModels.Automata.Elements;

namespace WordModels.Automata
{
    public class NFA
    {
        protected States states;
        protected Alphabet sigma;
        protected State initialState;
        protected Transitions transitions;

        private States FinalStates => new States(states.Where(s => s.IsFinal));

        public NFA(States states, Alphabet sigma, State initialState, Transitions transitions)
        {
            this.states = states;
            this.sigma = sigma;
            this.initialState = initialState;
            this.transitions = transitions;
            if (!states.Any(s => s.Name.Equals(initialState.Name)))
                throw new ArgumentException("Initial state must be contained in states!");
        }

        public DFA Determinize()
        {
            if (IsDeterministic())
                return new DFA(states, sigma, initialState, transitions);
            States pStates = new States();
            State pInitialState = new State("{" + initialState.Name + "}", initialState.IsFinal);
            Transitions pTransitions = new Transitions();
            Queue<States> queue = new Queue<States>();
            queue.Enqueue(new States { initialState });
            State sink = new State("{}");
            bool sinkRequired = false;
            while (queue.Count > 0)
            {
                States current = queue.Dequeue();
                if (pStates.Any(s => s.ToString().Equals(current.ToString())))
                    continue;
                State curr = new State(current.ToString(), current.Any(s => s.IsFinal));
                pStates.Add(curr);
                foreach (string letter in sigma)
                {
                    States next = new States();
                    foreach (State s in current)
                    {
                        try
                        {
                            foreach (State rState in transitions[(s, letter)])
                                next.Add(rState);
                        }
                        catch (KeyNotFoundException) { }
                    }
                    if (next.Count() > 0)
                    {
                        queue.Enqueue(next);
                        pTransitions.Add(curr, letter, new State(next.ToString(), next.Any(s => s.IsFinal)));
                    }
                    else
                    {
                        sinkRequired = true;
                        pTransitions.Add(curr, letter, sink);
                    }
                }
            }
            if (sinkRequired)
            {
                pStates.Add(sink);
                foreach (string letter in sigma)
                    pTransitions.Add(sink, letter, sink);
            }
            return new DFA(pStates, sigma, pInitialState, pTransitions);
        }

        public bool IsDeterministic() => transitions.GroupBy(t => t.Key.Source)
                .All(group => group.Select(t => t.Key.Letter).Count() == sigma.Count() && !group.Any(t => t.Value.Count() > 1));

        public bool ContainsWord(string word)
        {
            States curr = new States(initialState);
            for (int i = 0; i < word.Length; i++)
            {
                States next = new States();
                foreach (State s in curr.ToList())
                {
                    try
                    {
                        next.UnionWith(transitions[(s, word[i].ToString())]);
                    }
                    catch (KeyNotFoundException) { }
                }
                Console.WriteLine($"{word[i]} - {next}");
                if (next.Count() == 0)
                    return false;
                curr = next;
            }
            return curr.Any(s => s.IsFinal);
        }

        public override string ToString() => $"({states}, {sigma}, {initialState}, {transitions}, {FinalStates})";
    }
}