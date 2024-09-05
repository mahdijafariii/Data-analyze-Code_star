namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface IHeaderValidator
{
    void ValidateHeaders(IEnumerable<string> headers, List<string> requiredHeaders);
}