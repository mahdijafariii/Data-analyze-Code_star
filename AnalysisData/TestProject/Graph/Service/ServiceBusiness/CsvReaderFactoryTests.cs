using System.Text;
using AnalysisData.Graph.Service.ServiceBusiness;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class CsvReaderFactoryTests
{
    [Fact]
    public void CreateCsvReader_ShouldReturnCsvReader()
    {
        // Arrange
        var csvContent = "Header1,Header2\nValue1,Value2";
        var file = Substitute.For<IFormFile>();
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
        file.OpenReadStream().Returns(stream);
            
        var csvReaderFactory = new CsvReaderFactory();

        // Act
        var csvReader = csvReaderFactory.CreateCsvReader(file);

        // Assert
        Assert.NotNull(csvReader);
        Assert.IsType<CsvReader>(csvReader);
    }
}