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
            bool result = false;

            foreach(Variable v in vars)
            {
                result = result && v.getTruthValue();
            }

            return result;
        }
    }
}
