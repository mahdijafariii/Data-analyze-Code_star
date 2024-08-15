namespace AnalysisData.Exception;
using System;


public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base(Resources.InvalidPasswordException)
    {
    }
}