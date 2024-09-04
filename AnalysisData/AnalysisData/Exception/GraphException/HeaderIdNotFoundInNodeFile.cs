namespace AnalysisData.Exception.GraphException;

public class HeaderIdNotFoundInNodeFile : ServiceException
{
    public HeaderIdNotFoundInNodeFile(IEnumerable<string> missingHeaders)
        : base(string.Format(Resources.HeaderIdNotFoundInNodeFile, string.Join(", ", missingHeaders)), StatusCodes.Status404NotFound)
    {
    }
}