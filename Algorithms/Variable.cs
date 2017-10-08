using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game
{
    class Variable
    {
        private char var;
        private bool truthValue;

        public Variable(char c)
        {
            var = c;
            this.truthValue = true;
        }

        public void setValue(bool truthValue)
        {
            this.truthValue = truthValue;
        }

        public bool getTruthValue()
        {
            return this.truthValue;
        }

        public char getVariable()
        {
            return this.var;
        }

        public void setAsComplement()
        {
            this.truthValue = !this.truthValue;
        }
    }
}
