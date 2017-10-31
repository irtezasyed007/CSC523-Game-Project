using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game
{

    class TruthTable
    {
        private int variableCount;
        private List<Variable> variables;
        private List<Variable> uniqueVariables = new List<Variable>();
        private Dictionary<int, string> truthValues = new Dictionary<int, string>();
        private string postfix;

        public TruthTable(List<Variable> variables, string postfix)
        {
            this.variables = variables;     
            this.postfix = postfix;
            init();          
        }

        private void init()
        {
            initUniqueVariables();
            this.variableCount = uniqueVariables.Count;
            generateInputValues();
        }

        private void initUniqueVariables()
        {
            List<char> localVars = new List<char>(); 

            foreach (Variable v in variables)
            {
                char c = v.getVariable();
                if (Char.IsUpper(c)) c = Char.ToLower(c);

                if (!localVars.Contains(c))
                {
                    localVars.Add(c);
                    Variable var = new Variable(c);
                    uniqueVariables.Add(var);
                }
            }
        }

        private void generateInputValues()
        {
            if (variableCount == 2) twoVariables();
            else if (variableCount == 3) threeVariables();
            else if (variableCount == 4) fourVariables();
        }

        private void twoVariables()
        {
            int row = 1;

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {

                    string val = x.ToString() + y.ToString();
                    truthValues.Add(row, val);
                    row++;
                }
            }
        }

        private void threeVariables()
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

        private void fourVariables()
        {
            int row = 1;

            for (int w = 0; w < 2; w++)
            {
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            string val = w.ToString() + x.ToString() + y.ToString() + z.ToString();
                            truthValues.Add(row, val);
                            row++; 
                        }
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
                char[] rowTruthValues = val.ToCharArray();                
                updateVariableTruthValues(rowTruthValues);

                bool result = solveRow();
                Console.WriteLine(val + "     " + result);
            }
        }

        private void updateVariableTruthValues(char[] newTruthValues)
        {
            int index = 0;
            foreach(Variable v in uniqueVariables)
            {
                //newTruthValues = {'0', '1', '0'}
                bool value = Convert.ToBoolean(Char.GetNumericValue(newTruthValues[index]));
                v.setValue(value);
                index++;
            }
        }

        private bool solveRow()
        {
            Stack<Term> infixStack = new Stack<Term>();
            Stack<char> postfixStack = generatePostfixStack();
            Dictionary<string, bool> termValues = new Dictionary<string, bool>();
            for (int i = 0; i < postfixStack.Count; i++)
            {
                char token = postfix[i];

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
                    Variable v = charToVariable(token);
                    infixStack.Push(new Term(v.getVariable().ToString(), v.getTruthValue()));
                }
            }

            return infixStack.Pop().getValue();
        }

        private Stack<char> generatePostfixStack()
        {
            Stack<char> localPostfix = new Stack<char>();

            for (int i = postfix.Length - 1; i >= 0; i--)
            {
                localPostfix.Push(postfix[i]);
            }

            return localPostfix;
        }

        private bool isOperator(char c)
        {
            if (c == '+' || c == '*') return true;
            else return false;
        }

        private Variable charToVariable(char c)
        {
            foreach (Variable v in uniqueVariables)
            {
                if (v.getVariable() == c) return v;
                else if (Char.ToUpper(v.getVariable()) == c) return new Variable(c, !v.getTruthValue());
            }

            return null;
        }

        private void viewStack(Stack<char> s)
        {
            int size = s.Count();
            char[] array = new char[size];
            s.CopyTo(array, 0);

            Console.WriteLine("----------");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine("Index: " + i + " :: Element: " + array[i]);
            }
            Console.WriteLine("----------");
        }

    }

}
