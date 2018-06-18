using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JonathanRynd.IdleMinePlanner
{
    class StateMachine
    {
        public string prompt;
        public uint max;
        public UpgradeID type;
        public Feedback f;

        public delegate void Feedback(uint upgradelevel);

        public StateMachine(string s, uint m, UpgradeID t, Feedback fee)
        {
            prompt = s;
            max = m;
            type = t;
            f = fee;
        }

    }
}
