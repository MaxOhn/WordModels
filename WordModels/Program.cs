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
            Alphabet sigma = new Alphabet { "a", "b" };

            /*
            Console.WriteLine("--- AUTOMATA ---");
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
            Console.WriteLine(d.IsDeterministic());
            DFA d1 = d.Determinize();
            Console.WriteLine(d1);
            Console.WriteLine(d1.IsDeterministic());
            //*/
            //*
            Console.WriteLine("--- GRAMMARS ---");
            HashSet<string> NTS = new HashSet<string> { "S", "A" };
            Alphabet TS = sigma;
            Rules rules = new Rules
            {
                { new RuleSide("S"), new RuleSide("a", "S") },
                { new RuleSide("S"), new RuleSide("A") },
                { new RuleSide("A"), new RuleSide("b", "A") },
                { new RuleSide("A"), new RuleSide("b", "A", "a") },
                { new RuleSide("A"), new RuleSide("") }
            };
            CFG g = new CFG(NTS, TS, "S", rules);
            Console.WriteLine(g);
            /*
            CFG g1 = g.GetCNF();
            Console.WriteLine(g1);
            //*/
        }
    }
}