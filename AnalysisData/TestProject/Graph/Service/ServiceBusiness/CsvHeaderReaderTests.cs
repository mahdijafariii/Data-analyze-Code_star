using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class CsvHeaderReaderTests
{
    private readonly ICsvReader _csvReader;
    private readonly CsvHeaderReader _sut;
    
    public CsvHeaderReaderTests()
    {
        _csvReader = Substitute.For<ICsvReader>();
        _sut = new CsvHeaderReader();
    }

    [Fact]
    public void ReadHeaders_ShouldReturnHeaders_WhenCsvHasRecords()
    {
        // Arrange
        _csvReader.Read().Returns(true);
        _csvReader.HeaderRecord.Returns(new[] { "Header1", "Header2", "Header3" });

        // Act
        var result = _sut.ReadHeaders(_csvReader);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Contains("Header1", result);
        Assert.Contains("Header2", result);
        Assert.Contains("Header3", result);
        _csvReader.Received(1).Read(); 
        _csvReader.Received(1).ReadHeader(); 
    }

    [Fact]
    public void ReadHeaders_ShouldReturnEmpty_WhenCsvHasNoRecords()
    {
        // Arrange
        _csvReader.Read().Returns(false);

        // Act
        var result = _sut.ReadHeaders(_csvReader);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _csvReader.Received(1).Read();
        _csvReader.DidNotReceive().ReadHeader(); 
    }
}