using System.Collections.Generic;
using System.Linq;

namespace DFA_Analyzer.Entities
{
    public class TransitionsMap : List<Transition>
    {
        public int Σ(int state, char alpha) => this[state].σ(state, alpha); // Returns the final state of the transition
        
        public override string ToString() => this.Aggregate("", (current, transition) => current + $" [ {transition} ] ");

        public int this[int i, char c] => this.Find(transition => transition.isΣ(i,c)).σ(i,c);
    }
}