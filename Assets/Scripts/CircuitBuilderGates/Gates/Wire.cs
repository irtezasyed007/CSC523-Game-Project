using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game
{
    class Wire
    {
        private bool value;

        public Wire(bool value)
        {
            this.value = value;
        }

        public bool getValue()
        {
            return this.value;
        }
    }
}
