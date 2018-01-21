using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;


public class Function
{
    //Two representations of the function
    private string function;
    private string postfix;
    private List<Variable> variables = new List<Variable>();
    private TruthTable truthTable;
    private string header = "";

    // ((A + B)*C)
    // ((A + (B*C)) + D)
    // ABC*+D+
    public Function(string function)
    {
        this.function = function;
        postfix = toPostfix();
        variables = identifyVariables();
        truthTable = new TruthTable(variables, postfix);
    }

    public String FunctionString
    {
        get { return this.function;  }
    }

    //Precondition: The function to format must be fully paranthesized
    //Postcondition: Return a function in postfix form
    //Invariant: {array.Length > 5}
    private string toPostfix()
    {
        Stack<char>operands = new Stack<char>();

        string output = "";

        char[] array = function.ToCharArray();

        for(int i = 0; i < array.Length; i++)
        {
            char c = array[i];

            switch (c)
            {
                case '(':
                    operands.Push(c);
                    break;
                case ')':
                    while(operands.Peek() != '(')
                    {
                        output += operands.Pop().ToString();
                    }

                    //Removing the last '(' that we no longer need
                    operands.Pop();
                    break;
                case '+':
                    //Invariant: LET k=operands.Peek()  Precedence of k MUST: (k = +) OR (k > +)  
                    while (operands.Peek() == '+' || operands.Peek() == '×')
                    {
                        output += operands.Pop().ToString();
                    }

                    operands.Push(c);
                    break;
                case '×':
                    //Invariant: LET k=operands.Peek()  Precedence of k MUST: (k = ×)  
                    while (operands.Peek() == '×' || operands.Peek() == '^')
                    {
                        output += operands.Pop().ToString();
                    }

                    operands.Push(c);
                    break;
                case '^':
                    while(operands.Peek() == '^' || operands.Peek() == '×' || operands.Peek() == '+')
                    {
                        output += operands.Pop().ToString();
                    }

                    operands.Push(c);
                    break;
                default:
                    output += c.ToString();
                    break;
            }

        }

        return output;
    }

    private List<Variable> identifyVariables()
    {
        char[] functionChars = function.ToCharArray();
        List<Variable> variables = new List<Variable>();
        List<char> uniqueVariables = new List<char>();

        for (int i = 0; i < functionChars.Length; i++)
        {
            char var = functionChars[i];

            if (Char.IsLetter(var))
            {
                var = char.ToLower(var);
                if (!uniqueVariables.Contains(var))
                {
                    uniqueVariables.Add(var);   
                }
            }
        }

        uniqueVariables.Sort();
        foreach(char c in uniqueVariables)
        {
            header += char.ToLower(c);
            Variable v = new Variable(c);
            variables.Add(v);
        }

        return variables;
    }

    public bool[] getTruthResults()
    {
        return truthTable.getExpectedResults();
    }

    public string getPostFix()
    {
        return this.postfix;
    }

    public void viewTruthTable()
    {
        Debug.Log(header);
        truthTable.generateTruthTable();
    }

    internal string getHeader()
    {
        return this.header;
    }
}

