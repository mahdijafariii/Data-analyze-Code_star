using AnalysisData.Services.GraphService.Business.CsvManager;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace TestProject.Services.GraphService.ServiceBusiness.CsvManager;

public class CsvReaderManagerTests
{
    private readonly ICsvHeaderReaderProcessor _csvHeaderReaderProcessor;
    private readonly IHeaderValidatorProcessor _headerValidatorProcessor;
    private readonly CsvReaderManager _sut;
    private readonly IFormFile _formFile;
    private readonly MemoryStream _memoryStream;

    public CsvReaderManagerTests()
    {
        _csvHeaderReaderProcessor = Substitute.For<ICsvHeaderReaderProcessor>();
        _headerValidatorProcessor = Substitute.For<IHeaderValidatorProcessor>();
        _sut = new CsvReaderManager(_csvHeaderReaderProcessor, _headerValidatorProcessor);
        
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
        Assert.IsType<CsvReaderProcessor>(result);
    }

    [Fact]
    public void ReadHeaders_ShouldReturnHeaders_WhenHeadersAreValid()
    {
        // Arrange
        var cCsvReader = Substitute.For<ICsvReaderProcessor>();
        var headers = new[] { "Header1", "Header2", "Header3" };
        var requiredHeaders = new List<string> { "Header1", "Header2" };

        _csvHeaderReaderProcessor.ReadHeaders(cCsvReader).Returns(headers);

        // Act
        var result = _sut.ReadHeaders(cCsvReader, requiredHeaders);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(headers, result);
        
        _headerValidatorProcessor.Received(1).ValidateHeaders(headers, requiredHeaders);
    }
}