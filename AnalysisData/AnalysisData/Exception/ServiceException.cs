namespace AnalysisData.Exception;

public class ServiceException : System.Exception
{
    public int StatusCode { get; }

    public ServiceException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}