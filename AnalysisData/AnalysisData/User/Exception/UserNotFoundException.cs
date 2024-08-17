namespace AnalysisData.Exception;
using System;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base(Resources.UserNotFoundException)
    {
    }
    
}