using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game
{
    public static class BooleanStringGenerator
    {
        public struct BoolVar
        {
            public char var;
            public bool isPrime;
            public BoolVar(char c, bool b)
            {
                var = c;
                isPrime = b;
            }
            public static BoolVar getRandom(Random rng)
            {
                char c = (char)rng.Next(97, 101);
                return new BoolVar(c, false);
            }
            public static BoolVar[] getRandomArray(Random rng)
            {
                int numVars = rng.Next(2, 5);
                BoolVar[] arr = new BoolVar[numVars];

                char c;
                for (int i = 0; i < numVars; i++)
                {
                    c = (char)(97 + i);
                    arr[i] = new BoolVar(c, false);
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

                bool isPrime, operation = Convert.ToBoolean(rng.Next(0, 2));    // 0 means addition, 1 means multiplication
                isPrime = Convert.ToBoolean(rng.Next(0, 2));
                string newStackString = '(' + (isPrime ? var1.var.ToString().ToUpper() : var1.var.ToString());
                if (operation)
                {
                    isPrime = Convert.ToBoolean(rng.Next(0, 2));
                    newStackString += '*' + (isPrime ? var2.var.ToString().ToUpper() : var2.var.ToString()) + ')';
                }
                else
                {
                    isPrime = Convert.ToBoolean(rng.Next(0, 2));
                    newStackString += '+' + (isPrime ? var2.var.ToString().ToUpper() : var2.var.ToString()) + ')';
                }

                return new Term(arr, operation, newStackString);
            }

            public static Term generateTerm(Random rng, BoolVar[] arr, Term aTerm)
            {
                BoolVar var1 = arr[rng.Next(0, arr.Length)];

                bool isPrime, operation = Convert.ToBoolean(rng.Next(0, 2));    // 0 means addition, 1 means multiplication
                string newStackString = '(' + aTerm.StackString;
                isPrime = Convert.ToBoolean(rng.Next(0, 2));
                if (operation)
                {
                    newStackString += '*' + (isPrime ? var1.var.ToString().ToUpper() : var1.var.ToString()) + ')';
                }
                else
                {
                    newStackString += '+' + (isPrime ? var1.var.ToString().ToUpper() : var1.var.ToString()) + ')';
                }
                
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

                return new Term(arr, operation, newStackString);
            }
        }

        public class Equation
        {
            private BoolVar[] vars;
            private string stackString;
            private string booleanFunction;
            private int numOperations;
            private int numPrimeVars;
            private char[] uniqueVars;  // "Unique" meaning different letters, but uppercase and lowercase are treated the same

            public Equation()
            {
                vars = null;
                stackString = null;
                booleanFunction = null;
                numOperations = -1;
                numPrimeVars = -1;
                uniqueVars = null;
            }
            public string StackString
            {
                get { return stackString; }
            }
            public BoolVar[] Vars
            {
                get { return vars; }
            }

            public string BooleanFunction
            {
                get { return booleanFunction; }
                set { booleanFunction = value; }
            }

            public int NumPrimeVars
            {
                get { return numPrimeVars; }
                set { numPrimeVars = value; }
            }

            public int NumOperations
            {
                get { return numOperations; }
                set { numOperations = value; }
            }

            public char[] UniqueVars
            {
                get { return uniqueVars; }
            }

            public Equation(string stackString)
            {
                this.stackString = stackString;
                InitializeEquation(stackString);           
            }

            private void InitializeEquation(string unpreppedString)
            {
                //if (unpreppedString.StartsWith("((") && unpreppedString.EndsWith("))"))
                //{
                //    unpreppedString = unpreppedString.Substring(1, unpreppedString.Length - 2);
                //}
                //if (unpreppedString.StartsWith("((") && unpreppedString.EndsWith("))"))
                //{
                //    unpreppedString = unpreppedString.Substring(1, unpreppedString.Length - 2);
                //}

                int numPrimeVars = 0, numOperations = 0;
                List<BoolVar> list = new List<BoolVar>();
                List<char> uniqueVars = new List<char>(4);
                Stack<char> stack = new Stack<char>();
                for (int i = 0; i < unpreppedString.Length; ++i)
                {
                    if (Convert.ToInt32(unpreppedString[i]) >= 97)   // Lowercase letter
                    {
                        if (!uniqueVars.Contains(Char.ToUpper(unpreppedString[i])))
                        {
                            uniqueVars.Add(Char.ToUpper(unpreppedString[i]));
                        }
                        list.Add(new BoolVar(unpreppedString[i], false));
                        stack.Push(Char.ToUpper(unpreppedString[i]));
                    }
                    else if(Convert.ToInt32(unpreppedString[i]) <= 90 && Convert.ToInt32(unpreppedString[i]) >= 65) // Uppercase letter
                    {
                        if (!uniqueVars.Contains(unpreppedString[i]))
                        {
                            uniqueVars.Add(unpreppedString[i]);
                        }
                        numPrimeVars++;
                        list.Add(new BoolVar(unpreppedString[i], true));
                        stack.Push(unpreppedString[i]);
                        stack.Push('\'');
                    }
                    else if(unpreppedString[i] == '+' || unpreppedString[i] == '*')
                    {
                        numOperations++;
                        stack.Push(' ');
                        stack.Push(unpreppedString[i]);
                        stack.Push(' ');
                    }
                    else   // Parenthesis
                    {
                        stack.Push(unpreppedString[i]);
                    }
                }

                char[] preppedCharArray = new char[stack.Count];
                for (int i = stack.Count - 1; i >= 0; --i)
                {
                    preppedCharArray[i] = stack.Pop();
                }
                this.numPrimeVars = numPrimeVars;
                this.numOperations = numOperations;
                this.vars = list.ToArray<BoolVar>();
                this.booleanFunction = new string(preppedCharArray);
                this.uniqueVars = uniqueVars.ToArray<char>();
            }
        }

        public static Equation generateBooleanString(int numTerms = 2)    // 
        {
            Random rng = new Random();
            BoolVar[] arr = BoolVar.getRandomArray(rng);

            int termsLeft = numTerms;
            if(termsLeft == 3)
            {
                termsLeft -= rng.Next(1, termsLeft);
            }
            else if(termsLeft == 4)
            {
                termsLeft -= rng.Next(2, termsLeft);
            }


            int chosen;
            Term[] terms;
            string final = "";
            bool b = Convert.ToBoolean(rng.Next(0, 2));
            if (termsLeft == 2)
            {
                terms = new Term[2];
                terms[0] = Term.generateTerm(rng, arr);
                terms[1] = Term.generateTerm(rng, arr);
                if(numTerms == 2)
                {
                    final = terms[0].combineTerms(rng, arr, terms[1]).StackString;
                }
                else if(numTerms == 3)
                {
                    terms[Convert.ToInt16(b)] = Term.generateTerm(rng, arr, terms[Convert.ToInt16(b)]);
                    final = terms[0].combineTerms(rng, arr, terms[1]).StackString;
                }
                else if (numTerms == 4)
                {
                    terms[0] = Term.generateTerm(rng, arr, terms[0]);
                    terms[1] = Term.generateTerm(rng, arr, terms[1]);
                    chosen = b == true ? 0 : 1;
                    final = terms[Convert.ToInt16(b)].combineTerms(rng, arr, terms[chosen]).StackString;
                }              
            }
            else if (termsLeft == 1 && numTerms >= 2)
            {
                terms = new Term[3];
                terms[0] = Term.generateTerm(rng, arr);
                terms[1] = Term.generateTerm(rng, arr);
                terms[2] = Term.generateTerm(rng, arr);
                chosen = rng.Next(0, 3);
                if (numTerms == 3)
                {
                    terms[chosen] = terms[chosen].combineTerms(rng, arr, terms[(chosen + 1) % 3]);
                    terms[(chosen + 2) % 3] = terms[(chosen + 2) % 3].combineTerms(rng, arr, terms[chosen]);
                    final = terms[(chosen + 2) % 3].StackString;
                }
                else if (numTerms == 4)
                {
                    terms[chosen] = Term.generateTerm(rng, arr, terms[(chosen + 1) % 2]);
                    chosen = rng.Next(0, 2);
                    terms[chosen] = terms[chosen].combineTerms(rng, arr, terms[2]);
                    terms[(chosen + 1) % 2] = terms[(chosen + 1) % 2].combineTerms(rng, arr, terms[chosen]);
                    final = terms[(chosen + 1) % 2].StackString;
                }   
            }

            Equation eq = new Equation(final);
            Console.WriteLine(eq.StackString);

            return eq;
        }
    }
}
