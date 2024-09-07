namespace AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;

public interface ICsvReaderProcessor
{
    bool Read();
    void ReadHeader();
    string[] HeaderRecord { get; }
    string GetField(string fieldName); 
}