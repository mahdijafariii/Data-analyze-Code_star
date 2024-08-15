namespace AnalysisData.Exception;

public class DuplicateUserException : System.Exception
{
    public DuplicateUserException() : base(Resources.DuplicateUserException)
    {
    }
}