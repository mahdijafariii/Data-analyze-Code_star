namespace AnalysisData.Exception;

public class PasswordMismatchException : System.Exception
{
    public PasswordMismatchException() : base(Resources.PasswordMismatchException )
    {
    }
}