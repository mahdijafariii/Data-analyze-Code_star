using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class HeaderValidator : IHeaderValidator
{
    public void ValidateHeaders(IEnumerable<string> headers, List<string> requiredHeaders)
    {
        var missingHeaders = requiredHeaders.Where(h => !headers.Contains(h)).ToList();
        if (missingHeaders.Any())
        {
            throw new HeaderIdNotFoundInNodeFile(missingHeaders);
        }
    }
}