using CSC_523_Game.Gates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game.Gates
{

    abstract class Gate
    {
        public Wire[] inputs;
        public Wire input;
        public bool output;

        public Gate(Wire[] inputs)
        {
            this.inputs = inputs;
        }

        public Gate(Wire input)
        {
            this.input = input;
        }

        public abstract Wire getOutputWire();

        protected abstract bool gateOperation();

    }
}