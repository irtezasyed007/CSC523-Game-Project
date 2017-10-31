using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game
{
    abstract class Gate
    {
        public Variable[] vars;

        public Gate(Variable[] vars)
        {
            this.vars = vars;
        }

        public abstract bool gateOperation();

        public abstract bool getResult();

    }
}
