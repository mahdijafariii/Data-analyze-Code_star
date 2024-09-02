using AnalysisData.EAV.Model;
using CsvHelper;

namespace AnalysisData.EAV.Service.Business.Abstraction;

public interface INodeRecordProcessor
{
    Task<IEnumerable<EntityNode>> ProcessEntityNodesAsync(CsvReader csv, IEnumerable<string> headers, string id,
        int fileId);
}