namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface ICsvReader
{
    bool Read();
    void ReadHeader();
    string[] HeaderRecord { get; }
    string GetField(string fieldName); 
}