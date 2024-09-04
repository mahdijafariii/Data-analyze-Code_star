using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class EdgeToDbServiceTests
{
    private readonly EdgeToDbService _edgeToDbService;
    private readonly IFromToProcessor _fromToProcessor;
    private readonly ICsvReaderFactory _csvReaderFactory;
    private readonly ICsvHeaderValidator _csvHeaderValidator;
    private readonly IEntityEdgeRecordProcessor _entityEdgeRecordProcessor;
    private readonly IValueEdgeProcessor _valueEdgeProcessor;

    public EdgeToDbServiceTests()
    {
        _fromToProcessor = Substitute.For<IFromToProcessor>();
        _csvReaderFactory = Substitute.For<ICsvReaderFactory>();
        _csvHeaderValidator = Substitute.For<ICsvHeaderValidator>();
        _entityEdgeRecordProcessor = Substitute.For<IEntityEdgeRecordProcessor>();
        _valueEdgeProcessor = Substitute.For<IValueEdgeProcessor>();

        _edgeToDbService = new EdgeToDbService(
            _fromToProcessor, 
            _csvReaderFactory, 
            _csvHeaderValidator,
            _entityEdgeRecordProcessor, 
            _valueEdgeProcessor);
    }

    [Fact]
    public async Task ProcessCsvFileAsync_ValidFile_ProcessesCorrectly()
    {
        // Arrange
        var file = Substitute.For<IFormFile>();
        var from = "Source";
        var to = "Destination";
        var requiredHeaders = new List<string> { from, to };
        IEnumerable<string> headers = new List<string> { "Header1", "Header2" };
        var entityEdges = new List<EntityEdge>();
        IEnumerable<EntityEdge> entityEdgeEnumerable = entityEdges;

        var textReader = Substitute.For<TextReader>();
        var csvReader = new CsvReader(textReader, System.Globalization.CultureInfo.InvariantCulture);

        _csvReaderFactory.CreateCsvReader(file).Returns(csvReader);
        _csvHeaderValidator.ReadAndValidateHeaders(csvReader, requiredHeaders).Returns(headers);
        _entityEdgeRecordProcessor.ProcessEntityEdgesAsync(csvReader, from, to)
            .Returns(Task.FromResult(entityEdgeEnumerable));

        // Act
        await _edgeToDbService.ProcessCsvFileAsync(file, from, to);

        // Assert
        await _fromToProcessor.Received(1).ProcessFromToAsync(Arg.Any<IEnumerable<string>>(), from, to);
        await _entityEdgeRecordProcessor.Received(1).ProcessEntityEdgesAsync(csvReader, from, to);
        await _valueEdgeProcessor.Received(1).ProcessEntityEdgeValuesAsync(
            csvReader, 
            Arg.Any<IEnumerable<string>>(), 
            from, 
            to, 
            entityEdges
        );


    }
}