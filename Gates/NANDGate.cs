using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game.Gates
{
    class NANDGate : ANDGate
    {

        public NANDGate(Variable [] vars) : base(vars)
        {

        }

        public override bool getResult()
        {
            return !base.gateOperation();
        }

    }
}
