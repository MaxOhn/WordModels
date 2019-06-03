using System.Collections.Generic;

namespace WordModels.Grammars.Elements
{
    public class RuleSide
    {
        public List<string> Symbols { get; set; }

        public RuleSide() => Symbols = new List<string>();

        public RuleSide(string symbol) => Symbols = new List<string> { symbol };

        public RuleSide(IEnumerable<string> symbols) => Symbols = new List<string>(symbols);

        public override string ToString() => string.Join("", Symbols);
    }
}
