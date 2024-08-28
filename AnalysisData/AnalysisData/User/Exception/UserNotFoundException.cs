namespace AnalysisData.Exception;
using System;

public class UserNotFoundException : ServiceException
{
    public UserNotFoundException() : base(Resources.UserNotFoundException, StatusCodes.Status404NotFound)
    {
    }
    
}