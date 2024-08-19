using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace AnalysisData.FileManage.Service.Business;

public class CsvReaderService : ICsvReaderService
{
    public CsvReader CreateCsvReader(IFormFile file)
    {
        var reader = new StreamReader(file.OpenReadStream());
        return new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
            HasHeaderRecord = true
        });
    }
    
    public string[] ReadHeaders(CsvReader csv)
    {
        csv.Read();
        csv.ReadHeader();
        return csv.Context.Reader.HeaderRecord;
    }
}