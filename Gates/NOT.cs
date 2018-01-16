using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC_523_Game.Gates
{
    class NOT : Gate
    {

        public NOT(Wire input) : base(input)
        {
            output = gateOperation();
        }

        public override Wire getOutputWire()
        {
            return new Wire(output);
        }

        protected override bool gateOperation()
        {
            return !input.getValue();
        }
    }
}
