using System.Collections.Generic;

namespace WordModels
{
    public class Alphabet : HashSet<string>
    {
        public override string ToString() => "{" + string.Join(", ", this) + "}";
    }
}
