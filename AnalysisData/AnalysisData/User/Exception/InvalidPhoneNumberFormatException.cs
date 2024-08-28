namespace AnalysisData.Exception;

public class InvalidPhoneNumberFormatException : ServiceException
{
    public InvalidPhoneNumberFormatException() : base(Resources.InvalidPhoneNumberFormatException,StatusCodes.Status401Unauthorized)
    {
    }
}