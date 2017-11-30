using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Variable
{
    private char var;
    private bool truthValue;

    public Variable(char c)
    {
        var = c;
        this.truthValue = true;
    }

    public Variable(char c, bool truthValue)
    {
        this.var = c;
        this.truthValue = truthValue;
    }

    public void setValue(bool truthValue)
    {
        this.truthValue = truthValue;
    }

    public bool getTruthValue()
    {
        return this.truthValue;
    }

    public char getVariable()
    {
        return this.var;
    }

    public void setAsComplement()
    {
        this.truthValue = !this.truthValue;
    }
}

