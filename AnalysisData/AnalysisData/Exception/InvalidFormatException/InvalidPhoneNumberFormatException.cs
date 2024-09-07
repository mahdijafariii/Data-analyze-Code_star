namespace AnalysisData.Exception.InvalidFormatException;

public class InvalidPhoneNumberFormatException : ServiceException
{
    public InvalidPhoneNumberFormatException() : base(Resources.InvalidPhoneNumberFormatException,
        StatusCodes.Status400BadRequest)
    {
    }
}