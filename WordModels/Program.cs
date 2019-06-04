using System;
using System.Collections.Generic;
using WordModels.Automata;
using WordModels.Automata.Elements;
using WordModels.Grammars;
using WordModels.Grammars.Elements;

namespace WordModels
{
    class Program
    {
        static void Main(string[] args)
        {
            string word = "baabab";
            //*
            Console.WriteLine("--- AUTOMATA ---");
            Alphabet sigma = new Alphabet { "a", "b" };
            State q0 = new State("q0");
            State q1 = new State("q1", true);
            State q2 = new State("q2");
            States states = new States { q0, q1, q2 };
            Transitions transitions = new Transitions
            {
                { q0, "a", q1 },
                { q0, "b", q0 },
                { q0, "b", q1 },
                { q1, "a", q2 },
                { q1, "b", q2 },
                //{ q2, "b", q1 },
                { q2, "a", q0 }
            };
            NFA d = new NFA(states, sigma, q0, transitions);
            Console.WriteLine(d);
            Console.WriteLine($"Is deterministic: {d.IsDeterministic()}");
            DFA d1 = d.Determinize();
            Console.WriteLine(d1);
            Console.WriteLine($"Is deterministic: {d1.IsDeterministic()}");
            Console.WriteLine($"Contains word {word}: {d.ContainsWord(word)}");
            //*/
            //*
            Console.WriteLine("--- GRAMMARS ---");
            HashSet<string> NTS = new HashSet<string> { "S", "A", "B", "C" };
            Alphabet TS = new Alphabet { "a", "b", "c"};
            Rules rules = new Rules
            {
                { new RuleSide("S"), new RuleSide("A", "B") },
                { new RuleSide("S"), new RuleSide("a", "b", "C") },
                { new RuleSide("S"), new RuleSide("B") },
                { new RuleSide("A"), new RuleSide("C") },
                { new RuleSide("A"), new RuleSide("a", "a") },
                { new RuleSide("A"), new RuleSide("a", "A", "B") },
                { new RuleSide("B"), new RuleSide("b") },
                { new RuleSide("B"), new RuleSide("B", "B", "S") },
                { new RuleSide("C"), new RuleSide("B") },
                { new RuleSide("C"), new RuleSide("c", "c") }
            };
            CFG g = new CFG(NTS, TS, "S", rules);
            Console.WriteLine(g);
            Console.WriteLine($"Is in CNF: {g.IsInCNF()}");
            g.CNF();
            Console.WriteLine(g);
            Console.WriteLine($"Is in CNF: {g.IsInCNF()}");
            Console.WriteLine($"Contains word {word}: {g.ContainsWord(word)}");
            //*/
        }
    }
}