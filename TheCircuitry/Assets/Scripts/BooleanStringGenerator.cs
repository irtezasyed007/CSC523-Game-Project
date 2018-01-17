using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BooleanStringGenerator
{

    public  struct BoolVar
    {
        public readonly char var;
        public readonly bool isPrime;
        BoolVar(char c, bool b)
        {
            var = c;
            isPrime = b;
        }
        public static BoolVar getRandom(System.Random rng)
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

        public static BoolVar[] getRandomArray(System.Random rng)
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

        public int Operation
        {
            get { return operation; }
        }

        public static Term generateTerm(System.Random rng, BoolVar[] arr)
        {
            List<BoolVar> list = new List<BoolVar>(arr);
            BoolVar var1 = list.ElementAt(rng.Next(0, list.Count));
            list.Remove(var1);
            BoolVar var2 = list.ElementAt(rng.Next(0, list.Count));

            int generateOperation = rng.Next(0, 9);    // 0-3 = addition, 4-7 = multiplication, 8-9 = exclusive or
            string newStackString;
            char operation;
            if (generateOperation >= 0 && generateOperation <= 3)
            {
                operation = '+';
            }
            else if(generateOperation >= 4 && generateOperation <= 7)
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

        public static Term generateTerm(System.Random rng, BoolVar[] arr, Term aTerm)
        {
            BoolVar var1 = arr[rng.Next(0, arr.Length)];

            int generateOperation = rng.Next(0, 9);    // 0-3 = addition, 4-7 = multiplication, 8-9 = exclusive or
            string newStackString;
            char operation;
            if (generateOperation >= 0 && generateOperation <= 3)
            {
                operation = '+';
                
            }
            else if(generateOperation >= 4 && generateOperation <= 7)
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
        public Term combineTerms(System.Random rng, BoolVar[] arr, Term anotherTerm)
        {
            int generateOperation = rng.Next(0, 9);    // 0-3 = addition, 4-7 = multiplication, 8-9 = exclusive or
            string newStackString;
            char operation;
            if (generateOperation >= 0 && generateOperation <= 3)
            {
                operation = '+';
                newStackString = '(' + this.stackString + '*' + anotherTerm.StackString + ')';
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

    public static Equation generateBooleanString()    // We've limited the number of terms to 4
    {
        System.Random rng = new System.Random();
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
