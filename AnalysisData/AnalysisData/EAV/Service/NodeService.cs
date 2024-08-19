using System.Globalization;
using System.Text;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.FileManage.Service.Business;
using CsvHelper;
using CsvHelper.Configuration;

namespace AnalysisData.FileManage.Service;

public class NodeService : INodeService
{
    private readonly ICsvReaderService _csvReaderService;
    private readonly IHeaderProcessor _headerProcessor;
    private readonly IRecordProcessor _recordProcessor;

    public NodeService(ICsvReaderService csvReaderService, IHeaderProcessor headerProcessor, IRecordProcessor recordProcessor)
    {
        _csvReaderService = csvReaderService;
        _headerProcessor = headerProcessor;
        _recordProcessor = recordProcessor;
    }


    public async Task ProcessCsvFileAsync(IFormFile file, string id)
    {
        var csv = _csvReaderService.CreateCsvReader(file);
        var headers = _csvReaderService.ReadHeaders(csv);

        await _headerProcessor.ProcessHeadersAsync(headers, id);
        await _recordProcessor.ProcessRecordsAsync(csv, headers, id);
    }
    
}
