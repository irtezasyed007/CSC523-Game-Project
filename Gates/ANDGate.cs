using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC_523_Game
{
    class ANDGate : Gate
    {
        public ANDGate(Variable [] vars) : base(vars)
        {

        }

        public override bool getResult()
        {
            bool assignedValue = false;
            bool result = false;

            foreach (Variable v in Variables)
            {
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
