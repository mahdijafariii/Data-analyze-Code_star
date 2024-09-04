namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface ICsvReader
{
    bool Read();
    string GetField(string name);
}