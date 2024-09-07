using System.Security.Claims;
using AnalysisData.Graph.Model.File;
using AnalysisData.Graph.Repository.FileUploadedRepository.Abstraction;
using AnalysisData.Graph.Service.FileUploadService;
using NSubstitute;

namespace TestProject.Graph.Service.FileUploadServiceTests;

public class UploadFileServiceTests
{
    private readonly IFileUploadedRepository _fileUploadedRepository;
    private readonly UploadFileService _sut;

    public UploadFileServiceTests()
    {
        _fileUploadedRepository = Substitute.For<IFileUploadedRepository>();
        _sut = new UploadFileService(_fileUploadedRepository);
    }

    [Fact]
    public async Task AddFileToDb_ShouldAddFileToRepository_WhenCalled()
    {
        // Arrange
        var categoryId = 1;
        var userId = Guid.NewGuid();
        var fileName = "TestFile.txt";
        
        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("id", userId.ToString())
        }));

        // Act
        await _sut.AddFileToDb(categoryId, claims, fileName);

        // Assert
        await _fileUploadedRepository.Received(1).AddAsync(Arg.Is<FileEntity>(file =>
            file.UploaderId == userId &&
            file.CategoryId == categoryId &&
            file.FileName == fileName &&
            file.UploadDate <= DateTime.UtcNow 
        ));
    }
    [Fact]
    public async Task AddFileToDb_ShouldThrowFormatException_WhenInvalidUserId()
    {
        // Arrange
        var categoryId = 1;
        var invalidUserId = "invalid-guid";
        var fileName = "TestFile.txt";
        
        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("id", invalidUserId)
        }));

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => _sut.AddFileToDb(categoryId, claims, fileName));
    }
}