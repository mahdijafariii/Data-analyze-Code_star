using System.Text;
using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Service.ServiceBusiness;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
namespace TestProject.Graph.Service.ServiceBusiness;

public class CsvReaderServiceTests
{
    private readonly CsvReaderService _csvReaderService;
    private readonly CsvReaderFactory _csvReaderFactory;
    private readonly CsvHeaderValidator _csvHeaderValidator;

    public CsvReaderServiceTests()
    {
        _csvReaderFactory = Substitute.For<CsvReaderFactory>();
        _csvHeaderValidator = Substitute.For<CsvHeaderValidator>();
        _csvReaderService = new CsvReaderService(_csvReaderFactory, _csvHeaderValidator);
    }

    [Fact]
    public void ValidateCsvHeaders_ValidHeaders_ReturnsEmptyList()
    {

    }

    [Fact]
    public void ValidateCsvHeaders_InvalidHeaders_ReturnsMissingHeaders()
    {

    }
}