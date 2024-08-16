namespace AnalysisData.Exception;

public class InvalidEmailFormatException : System.Exception
{
    public InvalidEmailFormatException() : base(Resources.InvalidEmailFormatException)
    {
    }
}