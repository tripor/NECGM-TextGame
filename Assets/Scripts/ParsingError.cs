using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsingError : System.Exception
{

    private string error;

    public ParsingError(string error)
    {
        this.error = error;
    }

    override
    public string ToString()
    {
        return "Parsing error ocurred: " + error;
    }
}
