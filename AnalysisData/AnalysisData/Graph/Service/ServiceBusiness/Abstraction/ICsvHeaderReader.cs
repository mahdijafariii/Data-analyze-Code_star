namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface ICsvHeaderReader
{
    IEnumerable<string> ReadHeaders(ICsvReader csv);
}