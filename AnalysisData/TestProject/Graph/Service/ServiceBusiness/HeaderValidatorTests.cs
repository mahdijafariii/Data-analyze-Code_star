using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Service.ServiceBusiness;

namespace TestProject.Graph.Service.ServiceBusiness;

public class HeaderValidatorTests
{
    private readonly HeaderValidator _sut;

    public HeaderValidatorTests()
    {
        _sut = new HeaderValidator();
    }

    [Fact]
    public void ValidateHeaders_ShouldPass_WhenAllRequiredHeadersArePresent()
    {
        // Arrange
        var headers = new List<string> { "Header1", "Header2", "Header3" };
        var requiredHeaders = new List<string> { "Header1", "Header2" };

        // Act
        var exception = Record.Exception(() => _sut.ValidateHeaders(headers, requiredHeaders));
        
        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ValidateHeaders_ShouldThrow_WhenRequiredHeadersAreMissing()
    {
        // Arrange
        var headers = new List<string> { "Header1", "Header3" };
        var requiredHeaders = new List<string> { "Header1", "Header2", "Header3" };

        // Act & Assert
        var exception = Assert.Throws<HeaderIdNotFoundInNodeFile>(() => 
            _sut.ValidateHeaders(headers, requiredHeaders));
        
        Assert.Contains("Header2", exception.Message);
    }

    [Fact]
    public void ValidateHeaders_ShouldThrow_WhenAllRequiredHeadersAreMissing()
    {
        // Arrange
        var headers = new List<string> { "Header4", "Header5" };
        var requiredHeaders = new List<string> { "Header1", "Header2", "Header3" };

        // Act
        var exception = Assert.Throws<HeaderIdNotFoundInNodeFile>(() =>
            _sut.ValidateHeaders(headers, requiredHeaders));
        
        // Assert
        Assert.Contains("Header1", exception.Message);
        Assert.Contains("Header2", exception.Message);
        Assert.Contains("Header3", exception.Message);
    }

    [Fact]
    public void ValidateHeaders_ShouldPass_WhenNoRequiredHeaders()
    {
        // Arrange
        var headers = new List<string> { "Header1", "Header2", "Header3" };
        var requiredHeaders = new List<string>();

        // Act
        var exception = Record.Exception(() => _sut.ValidateHeaders(headers, requiredHeaders));
        
        // Assert
        Assert.Null(exception);
    }
}