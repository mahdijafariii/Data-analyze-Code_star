namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface INodeToDbService
{
    Task ProcessCsvFileAsync(IFormFile file, string id, int fileId);
}