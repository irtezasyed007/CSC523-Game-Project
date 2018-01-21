using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TruthTable
{
    private int variableCount;
    private List<Variable> variables;
    private List<Variable> uniqueVariables = new List<Variable>();
    private Dictionary<int, string> truthValues = new Dictionary<int, string>();
    private string postfix;
    private bool[] expectedResults;

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
        int row = 1;
        double inputs = Math.Pow(2, Convert.ToDouble(uniqueVariables.Count));
        expectedResults = new bool[(int) inputs];

        for(int i = 0; i < inputs; i++)
        {
            string binNum = Convert.ToString(i, 2);

            while(binNum.Length != uniqueVariables.Count)
            {
                binNum = "0" + binNum;
            }

            truthValues.Add(row, binNum);
            row++;
        }
    }

    //Should solve truth table
    public void generateTruthTable()
    {
        int index = 0;
        foreach (string val in truthValues.Values)
        {
            // {'0', '1', '0'}
            char[] rowTruthValues = val.ToCharArray();                
            updateVariableTruthValues(rowTruthValues);

            bool result = solveRow();
            expectedResults[index] = result;
            index++;
            Debug.Log(val + "     " + result);
        }
    }

    public bool[] getExpectedResults()
    {
        return this.expectedResults;
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

                else if(token == '×')
                {
                    string newTerm = t1.getExpression() + '×' + t2.getExpression();
                    bool newTermValue = t1.getValue() && t2.getValue();
                    Term term = new Term(newTerm, newTermValue);
                    infixStack.Push(term);
                }

                else if (token == '^')
                {
                    string newTerm = t1.getExpression() + '^' + t2.getExpression();
                    bool newTermValue = t1.getValue() ^ t2.getValue();
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
        if (c == '+' || c == '×' || c == '^') return true;
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


