using AnalysisData.Models.GraphModel.Node;

namespace AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;

public interface IHeaderProcessor
{
    Task<IEnumerable<AttributeNode>> ProcessHeadersAsync(IEnumerable<string> headers, string uniqueAttribute);
}