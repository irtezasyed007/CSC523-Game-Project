using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game
{
    class ANDGate : Gate
    {
        public ANDGate(Variable[] vars) : base(vars)
        {

        }

        public override bool getResult()
        {
            return gateOperation();
        }

        public override bool gateOperation()
        {
            bool assignedValue = false;
            bool result = false;

            foreach (Variable v in vars)
            {
                //Assigning a base value to the result
                if (!assignedValue)
                {
                    result = v.getTruthValue();
                    assignedValue = true;
                }

                else result = result && v.getTruthValue();
            }

            return result;
        }
    }
}
