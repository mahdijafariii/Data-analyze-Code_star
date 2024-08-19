namespace AnalysisData.FileManage.Service;

public interface INodeService
{
    Task ProcessCsvFileAsync(IFormFile file, string id);
}