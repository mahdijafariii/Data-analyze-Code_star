using System.Text;
using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Service.ServiceBusiness;
using CsvHelper;
using CsvHelper.Configuration;

namespace TestProject.Graph.Service.ServiceBusiness;

public class CsvHeaderValidatorTests
{
    [Fact]
    public void ReadAndValidateHeaders_ShouldReturnHeaders_WhenAllRequiredHeadersExist()
    {
        // Arrange
        var csvContent = "Header1,Header2\nValue1,Value2";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
        var reader = new StreamReader(stream);
        var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
        var csvReader = new CsvReader(reader, config);
            
        var requiredHeaders = new List<string> { "Header1", "Header2" };
        var csvHeaderValidator = new CsvHeaderValidator();

        // Act
        var headers = csvHeaderValidator.ReadAndValidateHeaders(csvReader, requiredHeaders);

        // Assert
        Assert.NotNull(headers);
        Assert.Contains("Header1", headers);
        Assert.Contains("Header2", headers);
    }

    [Fact]
    public void ReadAndValidateHeaders_ShouldThrowException_WhenRequiredHeadersAreMissing()
    {
        // Arrange
        var csvContent = "Header1,Header3\nValue1,Value2";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
        var reader = new StreamReader(stream);
        var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
        var csvReader = new CsvReader(reader, config);
            
        var requiredHeaders = new List<string> { "Header1", "Header2" };
        var csvHeaderValidator = new CsvHeaderValidator();

        // Act & Assert
        var exception = Assert.Throws<HeaderIdNotFoundInNodeFile>(() => 
            csvHeaderValidator.ReadAndValidateHeaders(csvReader, requiredHeaders));
        
        Assert.Contains("Header2", exception.Message);
        Assert.Equal(404, exception.StatusCode);
    }
}