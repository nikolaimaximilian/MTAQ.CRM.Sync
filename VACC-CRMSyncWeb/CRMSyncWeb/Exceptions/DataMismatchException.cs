using System;

public class DataMismatchException : Exception
{
    public DataMismatchException()
    {
    }

    public DataMismatchException(string message)
        : base(message)
    {
    }

    public DataMismatchException(string message, Exception inner)
        : base(message, inner)
    {
    }
}