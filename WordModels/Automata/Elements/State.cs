
namespace WordModels.Automata.Elements
{
    public class State
    {
        public string Name { get; }
        public bool IsFinal { get; set; }

        public State(string name) : this(name, false) { }

        public State(string Name, bool IsFinal)
        {
            this.Name = Name;
            this.IsFinal = IsFinal;
        }

        public override string ToString() => Name;
    }
}