namespace AnalysisData.EAV.Service.Abstraction;

public interface INodeService
{
    Task ProcessCsvFileAsync(IFormFile file, string id);
}