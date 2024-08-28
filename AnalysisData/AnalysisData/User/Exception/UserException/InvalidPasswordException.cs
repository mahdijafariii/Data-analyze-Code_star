namespace AnalysisData.Exception;
using System;


public class InvalidPasswordException : ServiceException
{
    public InvalidPasswordException() : base(Resources.InvalidPasswordException,StatusCodes.Status400BadRequest)
    {
    }
}