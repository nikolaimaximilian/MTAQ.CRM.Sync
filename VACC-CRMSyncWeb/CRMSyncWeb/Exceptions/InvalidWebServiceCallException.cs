using System;

public class InvalidWebServiceCallException : Exception
{
    public InvalidWebServiceCallException()
    {
    }

    public InvalidWebServiceCallException(string message)
        : base(message)
    {
    }

    public InvalidWebServiceCallException(string message, Exception inner)
        : base(message, inner)
    {
    }
}