namespace AnalysisData.DataProcessService;

public interface IDataProcessor
{
    Task ProcessDataAsync(Stream fileStream, string fileType);
}