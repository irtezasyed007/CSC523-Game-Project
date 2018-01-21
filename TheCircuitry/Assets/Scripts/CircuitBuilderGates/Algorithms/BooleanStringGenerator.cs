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
                bool b = Convert.ToBoolean(rng.Next(0, 2));
                if (b)
                {
                    c = (char)(Convert.ToInt16(c) - 32);
                }

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
            private char operation;
            private string stackString;

            public Term()
            {
                vars = null;
                stackString = null;
            }

            public Term(BoolVar[] vars, char operation, string stackString)
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
            public char Operation
            {
                get { return operation; }
            }
            public static Term generateTerm(Random rng, BoolVar[] arr)
            {
                List<BoolVar> list = new List<BoolVar>(arr);
                BoolVar var1 = list.ElementAt(rng.Next(0, list.Count));
                list.Remove(var1);
                BoolVar var2 = list.ElementAt(rng.Next(0, list.Count));

                int generateOperation = rng.Next(0, 10);    // 0-3 = addition, 4-7 = multiplication, 8-9 = exclusive or
                string newStackString;
                char operation;
                if (generateOperation >= 0 && generateOperation <= 3)
                {
                    operation = '+';
                }
                else if (generateOperation >= 4 && generateOperation <= 7)
                {
                    operation = '×';
                }
                else
                {
                    operation = '^';
                }
                newStackString = '(' + var1.var.ToString() + operation + var2.var.ToString() + ')';
                Console.WriteLine(newStackString);

                return new Term(arr, operation, newStackString);
            }

            public static Term generateTerm(Random rng, BoolVar[] arr, Term aTerm)
            {
                BoolVar var1 = arr[rng.Next(0, arr.Length)];

                int generateOperation = rng.Next(0, 10);    // 0-3 = addition, 4-7 = multiplication, 8-9 = exclusive or
                string newStackString;
                char operation;
                if (generateOperation >= 0 && generateOperation <= 3)
                {
                    operation = '+';

                }
                else if (generateOperation >= 4 && generateOperation <= 7)
                {
                    operation = '×';
                }
                else
                {
                    operation = '^';
                }
                newStackString = '(' + aTerm.StackString + operation + var1.var.ToString() + ')';
                Console.WriteLine(newStackString);

                return new Term(arr, operation, newStackString);
            }
            public Term combineTerms(Random rng, BoolVar[] arr, Term anotherTerm)
            {
                int generateOperation = rng.Next(0, 10);    // 0-3 = addition, 4-7 = multiplication, 8-9 = exclusive or
                string newStackString;
                char operation;
                if (generateOperation >= 0 && generateOperation <= 3)
                {
                    operation = '+';
                }
                else if (generateOperation >= 4 && generateOperation <= 7)
                {
                    operation = '×';
                }
                else
                {
                    operation = '^';
                }
                newStackString = '(' + this.stackString + operation + anotherTerm.StackString + ')';
                Console.WriteLine(newStackString);
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
                    if (Convert.ToInt32(unpreppedString[i]) >= 97 && unpreppedString[i] != '×')   // Lowercase letter
                    {
                        if (!uniqueVars.Contains(Char.ToUpper(unpreppedString[i])))
                        {
                            uniqueVars.Add(Char.ToUpper(unpreppedString[i]));
                        }
                        list.Add(new BoolVar(unpreppedString[i], false));
                        stack.Push(Char.ToUpper(unpreppedString[i]));
                    }
                    else if(Convert.ToInt32(unpreppedString[i]) <= 90 && Convert.ToInt32(unpreppedString[i]) >= 65)      // Uppercase letter
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
                    else if(unpreppedString[i] == '+' || unpreppedString[i] == '×' || unpreppedString[i] == '^')
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

        public static Equation generateBooleanString(int numTerms = 1)    // 
        {
            Random rng = new Random();
            BoolVar[] arr = BoolVar.getRandomArray(rng);

            int termsLeft = numTerms;
            if(termsLeft == 1)
            {
                Term term = Term.generateTerm(rng, arr);
                Equation equation = new Equation(term.StackString);
                return equation;
            }
            else if(termsLeft == 3)
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

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//public class BooleanStringGenerator
//{

//    public struct BoolVar
//    {
//        public readonly char var;
//        public readonly bool isPrime;
//        BoolVar(char c, bool b)
//        {
//            var = c;
//            isPrime = b;
//        }
//        public static BoolVar getRandom(System.Random rng)
//        {
//            char c = (char)rng.Next(97, 101);
//            bool b = Convert.ToBoolean(rng.Next(0, 2));
//            if (b)
//            {
//                c = (char)(Convert.ToInt16(c) - 32);
//            }
//            Console.WriteLine(c + "---isPrime=>" + b);

//            return new BoolVar(c, b);
//        }

//        public static BoolVar[] getRandomArray(System.Random rng)
//        {
//            int numVars = rng.Next(2, 5);
//            BoolVar[] arr = new BoolVar[numVars];
//            Console.WriteLine(numVars);

//            char c;
//            bool b;
//            for (int i = 0; i < numVars; i++)
//            {
//                c = (char)(97 + i);
//                b = Convert.ToBoolean(rng.Next(0, 2));
//                if (b)
//                {
//                    c = (char)(Convert.ToInt16(c) - 32);
//                }
//                Console.WriteLine(c + "---isPrime=>" + b);
//                arr[i] = new BoolVar(c, b);
//            }

//            return arr;
//        }

//        public static bool operator ==(BoolVar a, BoolVar b)
//        {
//            return (a.var == b.var && a.isPrime == b.isPrime);
//        }

//        public static bool operator !=(BoolVar a, BoolVar b)
//        {
//            return !(a.var == b.var && a.isPrime == b.isPrime);
//        }
//    }

//    class Term
//    {
//        private BoolVar[] vars;
//        private char operation;
//        private string stackString;

//        public Term()
//        {
//            vars = null;
//            stackString = null;
//        }

//        public Term(BoolVar[] vars, char operation, string stackString)
//        {
//            this.vars = vars;
//            this.operation = operation;
//            this.stackString = stackString;
//        }

//        public BoolVar[] Vars
//        {
//            get { return vars; }
//        }

//        public string StackString
//        {
//            get { return stackString; }
//        }

//        public int Operation
//        {
//            get { return operation; }
//        }

//        public static Term generateTerm(System.Random rng, BoolVar[] arr)
//        {
//            List<BoolVar> list = new List<BoolVar>(arr);
//            BoolVar var1 = list.ElementAt(rng.Next(0, list.Count));
//            list.Remove(var1);
//            BoolVar var2 = list.ElementAt(rng.Next(0, list.Count));

//            int generateOperation = rng.Next(0, 9);    // 0-3 = addition, 4-7 = multiplication, 8-9 = exclusive or
//            string newStackString;
//            char operation;
//            if (generateOperation >= 0 && generateOperation <= 3)
//            {
//                operation = '+';
//            }
//            else if (generateOperation >= 4 && generateOperation <= 7)
//            {
//                operation = '×';
//            }
//            else
//            {
//                operation = '^';
//            }
//            newStackString = '(' + var1.var.ToString() + operation + var2.var.ToString() + ')';
//            Console.WriteLine(newStackString);

//            return new Term(arr, operation, newStackString);
//        }

//        public static Term generateTerm(System.Random rng, BoolVar[] arr, Term aTerm)
//        {
//            BoolVar var1 = arr[rng.Next(0, arr.Length)];

//            int generateOperation = rng.Next(0, 9);    // 0-3 = addition, 4-7 = multiplication, 8-9 = exclusive or
//            string newStackString;
//            char operation;
//            if (generateOperation >= 0 && generateOperation <= 3)
//            {
//                operation = '+';

//            }
//            else if (generateOperation >= 4 && generateOperation <= 7)
//            {
//                operation = '×';
//            }
//            else
//            {
//                operation = '^';
//            }
//            newStackString = '(' + aTerm.StackString + operation + var1.var.ToString() + ')';
//            Console.WriteLine(newStackString);

//            return new Term(arr, operation, newStackString);
//        }
//        public Term combineTerms(System.Random rng, BoolVar[] arr, Term anotherTerm)
//        {
//            int generateOperation = rng.Next(0, 9);    // 0-3 = addition, 4-7 = multiplication, 8-9 = exclusive or
//            string newStackString;
//            char operation;
//            if (generateOperation >= 0 && generateOperation <= 3)
//            {
//                operation = '+';
//                newStackString = '(' + this.stackString + '*' + anotherTerm.StackString + ')';
//            }
//            else if (generateOperation >= 4 && generateOperation <= 7)
//            {
//                operation = '×';
//            }
//            else
//            {
//                operation = '^';
//            }
//            newStackString = '(' + this.stackString + operation + anotherTerm.StackString + ')';
//            Console.WriteLine(newStackString);
//            return new Term(arr, operation, newStackString);
//        }
//    }

//    public class Equation
//    {
//        private BoolVar[] vars;
//        private string stackString;

//        public Equation()
//        {
//            vars = null;
//            stackString = null;
//        }
//        public string StackString
//        {
//            get { return stackString; }
//        }
//        public BoolVar[] Vars
//        {
//            get { return vars; }
//        }
//        public Equation(BoolVar[] vars, string stackString)
//        {
//            this.vars = vars;
//            this.stackString = stackString;
//        }
//    }

//    public static Equation generateBooleanString()    // We've limited the number of terms to 4
//    {
//        System.Random rng = new System.Random();
//        BoolVar[] arr = BoolVar.getRandomArray(rng);

//        int termsLeft = 4;
//        int firstLevelTerms = rng.Next(2, 4);     // First Level Terms are the outermost in terms of parenthesis
//        termsLeft -= firstLevelTerms;


//        int chosen;
//        Term[] terms;
//        string final;
//        bool b = Convert.ToBoolean(rng.Next(0, 2));
//        if (termsLeft == 2)
//        {
//            terms = new Term[2];
//            terms[0] = Term.generateTerm(rng, arr);
//            terms[1] = Term.generateTerm(rng, arr);
//            terms[0] = Term.generateTerm(rng, arr, terms[0]);
//            terms[1] = Term.generateTerm(rng, arr, terms[1]);
//            b = Convert.ToBoolean(rng.Next(0, 2));
//            chosen = b == true ? 0 : 1;
//            final = terms[Convert.ToInt16(b)].combineTerms(rng, arr, terms[chosen]).StackString;
//            Console.WriteLine(true);
//        }
//        else    // termsLeft == 1
//        {
//            terms = new Term[3];
//            terms[0] = Term.generateTerm(rng, arr);
//            terms[1] = Term.generateTerm(rng, arr);
//            terms[2] = Term.generateTerm(rng, arr);
//            chosen = rng.Next(0, 3);
//            terms[chosen] = Term.generateTerm(rng, arr, terms[(chosen + 1) % 2]);
//            chosen = rng.Next(0, 2);
//            terms[chosen] = terms[chosen].combineTerms(rng, arr, terms[2]);
//            terms[(chosen + 1) % 2] = terms[(chosen + 1) % 2].combineTerms(rng, arr, terms[chosen]);
//            final = terms[(chosen + 1) % 2].StackString;
//            Console.WriteLine(false);
//        }
//        Equation eq = new Equation(arr, final);
//        Console.WriteLine(eq.StackString);

//        return eq;
//    }
//}

