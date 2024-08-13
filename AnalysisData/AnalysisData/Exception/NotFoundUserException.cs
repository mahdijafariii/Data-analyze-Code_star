namespace AnalysisData.Exception;
using System;

public class NotFoundUserException : Exception
{
    public NotFoundUserException() : base(Resources.NotFoundUserException , new NotFoundUserException())
    {
    }
}