using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game.Gates
{
    class NOR : OR
    {

        public NOR(Wire [] inputs) : base(inputs)
        {
            output = !gateOperation();
        }

        public override Wire getOutputWire()
        {
            return new Wire(output);
        }

    }
}
