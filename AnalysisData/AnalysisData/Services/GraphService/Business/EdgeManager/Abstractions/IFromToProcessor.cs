using AnalysisData.Models.GraphModel.Edge;

namespace AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;

public interface IFromToProcessor
{
    Task<IEnumerable<AttributeEdge>> ProcessFromToAsync(IEnumerable<string> headers, string from, string to);
}