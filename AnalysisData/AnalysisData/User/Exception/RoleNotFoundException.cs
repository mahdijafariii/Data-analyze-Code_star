namespace AnalysisData.Exception;

public class RoleNotFoundException : System.Exception
{
    public RoleNotFoundException() : base(Resources.RoleNotFoundException)
    {
    }
}