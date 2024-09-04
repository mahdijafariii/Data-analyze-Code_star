namespace AnalysisData.Exception.UserException;

public class UserNotFoundException : ServiceException
{
    public UserNotFoundException() : base(Resources.UserNotFoundException, StatusCodes.Status404NotFound)
    {
    }
}