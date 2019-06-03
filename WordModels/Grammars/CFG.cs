using System;
using System.Linq;
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
            // TODO
            return false;
        }

        public CFG GetCNF()
        {
            // Phase 0

            // TODO

            // Phase 1

            // TODO

            // Phase 2

            // TODO

            // Phase 3

            // TODO

            return this;
        }
        public bool IsInLanguage(string word)
        {
            if (!IsInCNF())
                throw new InvalidOperationException("Can only check language inclusion for grammars in CNF!");

            // TODO

            return false;
        }
    }
}