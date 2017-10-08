using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSC_523_Game
{

    class TruthTable
    {
        private int size;
        private List<Variable> variables;
        private Dictionary<int, string> truthValues = new Dictionary<int, string>();
        private string postfix;

        public TruthTable(List<Variable> variables, string postfix)
        {
            this.variables = variables;
            this.size = 2 ^ variables.Count;
            this.postfix = postfix;
            generateInputValues();
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
            char[] postfixCharArray = postfix.ToCharArray();
            Stack<string> s = new Stack<string>();
            Dictionary<string, bool> termValues = new Dictionary<string, bool>();
            bool truthValueResult = true;

            for (int i = 0; i < postfix.Count(); i++)
            {
                char c = postfixCharArray[i];
               
                switch (c)
                {
                    case '+':
                        string literal = s.Pop();
                        string trailingTerm = s.Pop();
                        bool trailingTermTruthValue;
                        string newTerm = "";

                        //If the dicitonary does not contain the truth value of the trailing term,
                        //create a new term and find that truth value then store back into stack and dictionary
                        if (!termValues.TryGetValue(trailingTerm, out trailingTermTruthValue))
                        {
                            Variable v1 = charToVariable(trailingTerm.ToCharArray()[0]);
                            Variable v2 = charToVariable(literal.ToCharArray()[0]);
                            trailingTermTruthValue = v1.getTruthValue() || v2.getTruthValue();
                            newTerm = trailingTerm + "+" + literal;
                            termValues.Add(newTerm, trailingTermTruthValue);
                            s.Push(newTerm);
                            break;
                        }

                        //If it does contain the truth value of the trailing term.
                        else
                        {
                            Variable v = charToVariable(literal.ToCharArray()[0]);
                            newTerm = trailingTerm + "+" + v.getVariable();
                            truthValueResult = trailingTermTruthValue || v.getTruthValue();
                            termValues.Add(newTerm, trailingTermTruthValue);
                            s.Push(newTerm);
                        }
                        break;
                    case '*':
                        literal = s.Pop();
                        trailingTerm = s.Pop();

                        //If the dicitonary does not contain the truth value of the trailing term,
                        //create a new term and find that truth value then store back into stack and dictionary
                        if (!termValues.TryGetValue(trailingTerm, out trailingTermTruthValue))
                        {
                            Variable v1 = charToVariable(trailingTerm.ToCharArray()[0]);
                            Variable v2 = charToVariable(literal.ToCharArray()[0]);
                            trailingTermTruthValue = v1.getTruthValue() && v2.getTruthValue();
                            newTerm = trailingTerm + "*" + literal;
                            termValues.Add(newTerm, trailingTermTruthValue);
                            s.Push(newTerm);
                            break;
                        }

                        //If it does contain the truth value of the trailing term.
                        else
                        {
                            Variable v = charToVariable(literal.ToCharArray()[0]);
                            newTerm = trailingTerm + "*" + v.getVariable();
                            truthValueResult = trailingTermTruthValue && v.getTruthValue();
                            termValues.Add(newTerm, trailingTermTruthValue);
                            s.Push(newTerm);
                        }
                        break;
                    default:
                        s.Push(c.ToString());
                        break;
                }
            }

            return truthValueResult;
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
