namespace AnalysisData.Exception;

public class PasswordHasherInputNull : ServiceException
{
    public PasswordHasherInputNull() : base(Resources.PasswordHasherInputNull, StatusCodes.Status400BadRequest)
    {
    }
}