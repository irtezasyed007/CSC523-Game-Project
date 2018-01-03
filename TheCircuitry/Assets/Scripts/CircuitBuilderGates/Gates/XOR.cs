using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game.Gates
{
    class XOR : Gate
    {

        public XOR(Wire [] inputs) : base(inputs)
        {
            output = gateOperation();
        }

        public override Wire getOutputWire()
        {
            return new Wire(output);
        }

        protected override bool gateOperation()
        {
            bool assignedValue = false;
            bool result = false;

            foreach (Wire w in inputs)
            {
                if (!assignedValue)
                {
                    result = w.getValue();
                    assignedValue = true;
                }

                else result = result ^ w.getValue();
            }

            return result;
        }

    }
}
