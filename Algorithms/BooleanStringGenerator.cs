using CSC_523_Game.Gates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC_523_Game
{
    class BooleanStringGenerator
    {
        static void Main(string[] args)
        {
            Wire w1 = new Wire(true);
            Wire w2 = new Wire(false);
            Wire[] inputs = { w1, w2 };
            writeLine("AND");
            writeLine(new AND(inputs).getOutputWire().getValue());
            writeLine("");
            writeLine("NAND");
            writeLine(new NAND(inputs).getOutputWire().getValue());
            writeLine("");
            writeLine("NOR");
            writeLine(new NOR(inputs).getOutputWire().getValue());
            writeLine("");
            writeLine("OR");
            writeLine(new OR(inputs).getOutputWire().getValue());
            writeLine("");
            writeLine("XNOR");
            writeLine(new XNOR(inputs).getOutputWire().getValue());
            writeLine("");
            writeLine("XOR");
            writeLine(new XOR(inputs).getOutputWire().getValue());
            writeLine("");
            writeLine("NOT");
            writeLine(new NOT(w1).getOutputWire().getValue());
            Console.ReadKey();
        }

        public static void writeLine(Object val)
        {
            Console.WriteLine(val);
        }

        public struct BoolVar
        {
            public readonly char var;
            public readonly bool isPrime;
            BoolVar(char c, bool b)
            {
                var = c;
                isPrime = b;
            }
            public static BoolVar getRandom(Random rng)
            {
                char c = (char)rng.Next(97, 101);
                bool b = Convert.ToBoolean(rng.Next(0, 2));
                if (b)
                {
                    c = (char)(Convert.ToInt16(c) - 32);
                }
                Console.WriteLine(c + "---isPrime=>" + b);

                return new BoolVar(c, b);
            }
            public static BoolVar[] getRandomArray(Random rng)
            {
                int numVars = rng.Next(2, 5);
                BoolVar[] arr = new BoolVar[numVars];
                Console.WriteLine(numVars);

                char c;
                bool b;
                for (int i = 0; i < numVars; i++)
                {
                    c = (char)(97 + i);
                    b = Convert.ToBoolean(rng.Next(0, 2));
                    if (b)
                    {
                        c = (char)(Convert.ToInt16(c) - 32);
                    }
                    Console.WriteLine(c + "---isPrime=>" + b);
                    arr[i] = new BoolVar(c, b);
                }

                return arr;
            }

            public static bool operator ==(BoolVar a, BoolVar b)
            {
                return (a.var == b.var && a.isPrime == b.isPrime);
            }
            public static bool operator !=(BoolVar a, BoolVar b)
            {
                return !(a.var == b.var && a.isPrime == b.isPrime);
            }
        }

        class Term
        {
            private BoolVar[] vars;
            private bool operation;
            private string stackString;

            public Term()
            {
                vars = null;
                stackString = null;
            }

            public Term(BoolVar[] vars, bool operation, string stackString)
            {
                this.vars = vars;
                this.operation = operation;
                this.stackString = stackString;
            }
            public BoolVar[] Vars
            {
                get { return vars; }
            }
            public string StackString
            {
                get { return stackString; }
            }
            public bool Operation
            {
                get { return operation; }
            }
            public static Term generateTerm(Random rng, BoolVar[] arr)
            {
                List<BoolVar> list = new List<BoolVar>(arr);
                BoolVar var1 = list.ElementAt(rng.Next(0, list.Count));
                list.Remove(var1);
                BoolVar var2 = list.ElementAt(rng.Next(0, list.Count));

                bool operation = Convert.ToBoolean(rng.Next(0, 2));    // 0 means addition, 1 means multiplication
                string newStackString;
                if (operation)
                {
                    newStackString = '(' + var1.var.ToString() + '*' + var2.var.ToString() + ')';
                }
                else
                {
                    newStackString = '(' + var1.var.ToString() + '+' + var2.var.ToString() + ')';
                }
                Console.WriteLine(newStackString);


                return new Term(arr, operation, newStackString);
            }

            public static Term generateTerm(Random rng, BoolVar[] arr, Term aTerm)
            {
                BoolVar var1 = arr[rng.Next(0, arr.Length)];

                bool operation = Convert.ToBoolean(rng.Next(0, 2));    // 0 means addition, 1 means multiplication
                string newStackString;
                if (operation)
                {
                    newStackString = '(' + aTerm.StackString + '*' + var1.var.ToString() + ')';
                }
                else
                {
                    newStackString = '(' + aTerm.StackString + '+' + var1.var.ToString() + ')';
                }
                Console.WriteLine(newStackString);


                return new Term(arr, operation, newStackString);
            }
            public Term combineTerms(Random rng, BoolVar[] arr, Term anotherTerm)
            {
                bool operation = Convert.ToBoolean(rng.Next(0, 2));
                string newStackString;
                if (operation)  // multiplication
                {
                    newStackString = '(' + this.stackString + '*' + anotherTerm.StackString + ')';
                }
                else
                {
                    newStackString = '(' + this.stackString + '+' + anotherTerm.StackString + ')';
                }
                Console.WriteLine(newStackString);
                return new Term(arr, operation, newStackString);
            }
        }

        public class Equation
        {
            private BoolVar[] vars;
            private string stackString;

            public Equation()
            {
                vars = null;
                stackString = null;
            }
            public string StackString
            {
                get { return stackString; }
            }
            public BoolVar[] Vars
            {
                get { return vars; }
            }
            public Equation(BoolVar[] vars, string stackString)
            {
                this.vars = vars;
                this.stackString = stackString;
            }
        }

        static public Equation generateBooleanString()    // We've limited the number of terms to 4
        {
            Random rng = new Random();
            BoolVar[] arr = BoolVar.getRandomArray(rng);

            int termsLeft = 4;
            int firstLevelTerms = rng.Next(2, 4);     // First Level Terms are the outermost in terms of parenthesis
            termsLeft -= firstLevelTerms;


            int chosen;
            Term[] terms;
            string final;
            bool b = Convert.ToBoolean(rng.Next(0, 2));
            if (termsLeft == 2)
            {
                terms = new Term[2];
                terms[0] = Term.generateTerm(rng, arr);
                terms[1] = Term.generateTerm(rng, arr);
                terms[0] = Term.generateTerm(rng, arr, terms[0]);
                terms[1] = Term.generateTerm(rng, arr, terms[1]);
                b = Convert.ToBoolean(rng.Next(0, 2));
                chosen = b == true ? 0 : 1;
                final = terms[Convert.ToInt16(b)].combineTerms(rng, arr, terms[chosen]).StackString;
                Console.WriteLine(true);
            }
            else    // termsLeft == 1
            {
                terms = new Term[3];
                terms[0] = Term.generateTerm(rng, arr);
                terms[1] = Term.generateTerm(rng, arr);
                terms[2] = Term.generateTerm(rng, arr);
                chosen = rng.Next(0, 3);
                terms[chosen] = Term.generateTerm(rng, arr, terms[(chosen + 1) % 2]);
                chosen = rng.Next(0, 2);
                terms[chosen] = terms[chosen].combineTerms(rng, arr, terms[2]);
                terms[(chosen + 1) % 2] = terms[(chosen + 1) % 2].combineTerms(rng, arr, terms[chosen]);
                final = terms[(chosen + 1) % 2].StackString;
                Console.WriteLine(false);
            }
            Equation eq = new Equation(arr, final);
            Console.WriteLine(eq.StackString);

            return eq;
        }
    }
}