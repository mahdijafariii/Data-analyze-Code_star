using AnalysisData.Services.GraphService.Business.CsvManager;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class CsvHeaderReaderTests
{
    private readonly ICsvReaderProcessor _csvReaderProcessor;
    private readonly CsvHeaderReaderProcessor _sut;
    
    public CsvHeaderReaderTests()
    {
        _csvReaderProcessor = Substitute.For<ICsvReaderProcessor>();
        _sut = new CsvHeaderReaderProcessor();
    }

    [Fact]
    public void ReadHeaders_ShouldReturnHeaders_WhenCsvHasRecords()
    {
        // Arrange
        _csvReaderProcessor.Read().Returns(true);
        _csvReaderProcessor.HeaderRecord.Returns(new[] { "Header1", "Header2", "Header3" });

        // Act
        var result = _sut.ReadHeaders(_csvReaderProcessor);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Contains("Header1", result);
        Assert.Contains("Header2", result);
        Assert.Contains("Header3", result);
        _csvReaderProcessor.Received(1).Read(); 
        _csvReaderProcessor.Received(1).ReadHeader(); 
    }

    [Fact]
    public void ReadHeaders_ShouldReturnEmpty_WhenCsvHasNoRecords()
    {
        // Arrange
        _csvReaderProcessor.Read().Returns(false);

        // Act
        var result = _sut.ReadHeaders(_csvReaderProcessor);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _csvReaderProcessor.Received(1).Read();
        _csvReaderProcessor.DidNotReceive().ReadHeader(); 
    }
}