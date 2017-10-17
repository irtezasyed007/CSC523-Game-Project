using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsVsZombies_test
{
    class Program
    {
        static void Main(string[] args)
        {
            string aString = "aB+Ab";
            generateBooleanString();
            Console.ReadKey();
        }

        struct BoolVar
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
                    newStackString = '(' + var1.var.ToString() + '+' + var2.var.ToString() +  ')';
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


                return new Term();
            }
        }

        class Equation
        {
            private Term[] terms;

            public Equation()
            {
                terms = null;
            }
            public Equation(Term[] terms)
            {
                this.terms = terms;
            }
        }

        static public char[] generateBooleanString()    // We've limited the number of terms to 4
        {
            Random rng = new Random();
            BoolVar[] arr = BoolVar.getRandomArray(rng);

            int termsLeft = 4;
            int firstLevelTerms = rng.Next(2, 4);     // First Level Terms are the outermost in terms of parenthesis
            termsLeft -= firstLevelTerms;
            
            bool b = Convert.ToBoolean(rng.Next(0, 2));
            if (termsLeft == 2)
            {
                Term.generateTerm(rng, arr, Term.generateTerm(rng, arr));
            }
            else
            {
                Term.generateTerm(rng, arr, Term.generateTerm(rng, arr));
            }


            return new char[10];
        }
    }
}
