namespace AnalysisData.Graph.DataProcessService;

public interface IDataProcessor
{
    Task<List<string>> ProcessDataAsync(Stream fileStream, string fileType);
}