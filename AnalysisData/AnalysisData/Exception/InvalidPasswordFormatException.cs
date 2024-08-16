namespace AnalysisData.Exception;

public class InvalidPasswordFormatException : System.Exception
{
    public InvalidPasswordFormatException() : base(Resources.InvalidPasswordFormatException)
    {
    }
}