using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class CsvReaderServiceTests
{
    private readonly ICsvHeaderReader _csvHeaderReader;
    private readonly IHeaderValidator _headerValidator;
    private readonly CsvReaderService _sut;
    private readonly IFormFile _formFile;
    private readonly MemoryStream _memoryStream;

    public CsvReaderServiceTests()
    {
        _csvHeaderReader = Substitute.For<ICsvHeaderReader>();
        _headerValidator = Substitute.For<IHeaderValidator>();
        _sut = new CsvReaderService(_csvHeaderReader, _headerValidator);
        
        _formFile = Substitute.For<IFormFile>();
        _memoryStream = new MemoryStream();
        
        _formFile.OpenReadStream().Returns(_memoryStream);
    }

    [Fact]
    public void CreateCsvReader_ShouldReturnCsvReaderWrapper_WhenValidFileIsProvided()
    {
        // Act
        var result = _sut.CreateCsvReader(_formFile);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CsvReaderWrapper>(result);
    }

    [Fact]
    public void ReadHeaders_ShouldReturnHeaders_WhenHeadersAreValid()
    {
        // Arrange
        var cCsvReader = Substitute.For<ICsvReader>();
        var headers = new[] { "Header1", "Header2", "Header3" };
        var requiredHeaders = new List<string> { "Header1", "Header2" };

        _csvHeaderReader.ReadHeaders(cCsvReader).Returns(headers);

        // Act
        var result = _sut.ReadHeaders(cCsvReader, requiredHeaders);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(headers, result);
        
        _headerValidator.Received(1).ValidateHeaders(headers, requiredHeaders);
    }
}