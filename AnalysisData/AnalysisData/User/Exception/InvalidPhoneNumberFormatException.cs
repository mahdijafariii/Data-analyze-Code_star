namespace AnalysisData.Exception;

public class InvalidPhoneNumberFormatException : System.Exception
{
    public InvalidPhoneNumberFormatException() : base(Resources.InvalidPhoneNumberFormatException)
    {
    }
}