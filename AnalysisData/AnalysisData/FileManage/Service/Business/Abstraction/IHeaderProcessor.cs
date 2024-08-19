namespace AnalysisData.FileManage.Service.Business;

public interface IHeaderProcessor
{
    Task ProcessHeadersAsync(string[] headers, string uniqueAttribute);
}