namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface IEdgeToDbService
{
    Task ProcessCsvFileAsync(IFormFile file, string from, string to);
}