namespace AnalysisData.Exception;

public class DuplicateRoleExistException : System.Exception
{
    public DuplicateRoleExistException() : base(Resources.DuplicateRoleException)
    {
    }
}