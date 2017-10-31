using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC_523_Game
{
    class Term
    {
        private string expression;
        private bool value;

        public Term(string expression, bool value)
        {
            this.expression = expression;
            this.value = value;
        }

        public string getExpression()
        {
            return this.expression;
        }

        public bool getValue()
        {
            return this.value;
        }
    }
}
