using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication19
{

    class TruthTable
    {
        private int size;
        private List<Variable> variables;
        private Dictionary<int, string> truthValues = new Dictionary<int, string>();
        private string postfix;
        private Stack<char> postfixStack = new Stack<char>();

        public TruthTable(List<Variable> variables, string postfix)
        {
            this.variables = variables;
            this.size = 2 ^ variables.Count;
            this.postfix = postfix;
            initPostfixStack();
            generateInputValues();
        }

        private void initPostfixStack()
        {
            for (int i = 0; i < postfix.Length; i++)
            {
                postfixStack.Push(postfix[i]);
            }
        }
       
        private void generateInputValues()
        {
            int row = 1;

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        string val = x.ToString() + y.ToString() + z.ToString();
                        truthValues.Add(row, val);
                        row++;
                    }
                }
            }
        }

        //Should solve truth table
        public void generateTruthTable()
        {
            foreach (string val in truthValues.Values)
            {
                // {'0', '1', '0'}
                char[] boolVal = val.ToCharArray();
                updateVariableTruthValues(boolVal);

                bool result = solveRow();
                Console.WriteLine(val + "     " + result);
            }
        }

        private void updateVariableTruthValues(char[] newTruthValues)
        {
            int index = 0;
            foreach (Variable v in variables)
            {
                int charToInt = (int) Char.GetNumericValue(newTruthValues[index]);
                v.setValue(Convert.ToBoolean(charToInt));
                index++;
            }
        }

        private bool solveRow()
        {
            Stack<Term> infixStack = new Stack<Term>();
            Dictionary<string, bool> termValues = new Dictionary<string, bool>();
            bool truthValueResult = true;

            for (int i = 0; i < postfixStack.Count; i++)
            {
                char token = postfixStack.Pop();

                if (isOperator(token))
                {
                    Term t1 = infixStack.Pop();
                    Term t2 = infixStack.Pop();

                    if (token == '+')
                    {
                        string newTerm = t1.getExpression() + '+' + t2.getExpression();
                        bool newTermValue = t1.getValue() || t2.getValue();
                        Term term = new Term(newTerm, newTermValue);
                        infixStack.Push(term);
                    }

                    else
                    {
                        string newTerm = t1.getExpression() + '*' + t2.getExpression();
                        bool newTermValue = t1.getValue() && t2.getValue();
                        Term term = new Term(newTerm, newTermValue);
                        infixStack.Push(term);
                    }
                }

                else
                {
                    infixStack.Push(new Term(token.ToString(), true));
                }
            }

            return truthValueResult;
        }

        private bool isOperator(char c)
        {
            if (c == '+' || c == '*') return true;
            else return false;
        }

        private Variable charToVariable(char c)
        {
            bool isUppercase = false;
            if (Char.IsUpper(c)) isUppercase = true;

            c = Char.ToLower(c);

            foreach(Variable v in variables)
            {
                if (v.getVariable().Equals(c))
                {
                    if (isUppercase) v.setAsComplement();

                    return v;
                }
            }

            return null;
        }

    }

}
