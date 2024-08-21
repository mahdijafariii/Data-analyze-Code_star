namespace AnalysisData.Exception;

public class NoFileUploadedException : System.Exception
{
    public NoFileUploadedException() : base(Resources.NoFileUploadedException)
    {
    }
}