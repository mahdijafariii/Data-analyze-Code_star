namespace AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;

public interface INodeToDbProcessor
{
    Task ProcessCsvFileAsync(IFormFile file, string id, int fileId);
}