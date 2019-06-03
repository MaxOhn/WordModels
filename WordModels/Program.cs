using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordModels.Automata;
using WordModels.Automata.Elements;

namespace WordModels
{
    class Program
    {
        static void Main(string[] args)
        {
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
            Console.WriteLine(d.IsDeterministic());
            //*
            DFA d1 = d.Determinize();
            Console.WriteLine(d1);
            Console.WriteLine(d1.IsDeterministic());
            //*/
            /*
            System.Console.WriteLine("--- GRAMMARS ---");
            NonTerminalSymbols NTS = new NonTerminalSymbols { "S", "A" };
            TerminalSymbols TS = new TerminalSymbols { "a", "b" };
            Rules rules = new Rules
            {
                new Rule(new NonTerminalSymbols { "S" }, new Symbols { "a", "S" }),
                new Rule(new NonTerminalSymbols { "S" }, new Symbols { "A" }),
                new Rule(new NonTerminalSymbols { "A" }, new Symbols { "b", "A" }),
                new Rule(new NonTerminalSymbols { "A" }, new Symbols { "b", "A", "a" }),
                new Rule(new NonTerminalSymbols { "A" }, new Symbols( "" )),
            };
            CFG g = new CFG(NTS, TS, "S", rules);
            Console.WriteLine(g);
            CFG g1 = g.getCNF();
            Console.WriteLine(g1);
            //*/
        }
    }
}