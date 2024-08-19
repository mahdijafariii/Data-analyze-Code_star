using CsvHelper;

namespace AnalysisData.FileManage.Service.Business;

public interface ICsvReaderService
{
    public CsvReader CreateCsvReader(IFormFile file);
    public string[] ReadHeaders(CsvReader csv);
}