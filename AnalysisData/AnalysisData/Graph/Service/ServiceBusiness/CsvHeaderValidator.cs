using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class CsvHeaderValidator : ICsvHeaderValidator
{
    public IEnumerable<string> ReadAndValidateHeaders(CsvReader csv, List<string> requiredHeaders)
    {
        if (csv.Read())
        {
            csv.ReadHeader();
            var headers = csv.Context.Reader.HeaderRecord ?? Enumerable.Empty<string>();
                
            var missingHeaders = requiredHeaders.Where(h => !headers.Contains(h)).ToList();
            if (missingHeaders.Any())
            {
                throw new HeaderIdNotFoundInNodeFile(missingHeaders);
            }

            return headers;
        }

        return Enumerable.Empty<string>();
    }
}