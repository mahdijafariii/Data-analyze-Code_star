using AnalysisData.Exception.FileException;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager;

public class HeaderValidatorProcessorProcessor : IHeaderValidatorProcessor
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