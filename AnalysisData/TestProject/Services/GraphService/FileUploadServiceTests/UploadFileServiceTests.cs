using System.Security.Claims;
using AnalysisData.Exception.GraphException.CategoryException;
using AnalysisData.Models.GraphModel.Category;
using AnalysisData.Models.GraphModel.File;
using AnalysisData.Repositories.GraphRepositories.CategoryRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.FileUploadedRepository.Abstraction;
using AnalysisData.Services.GraphService.FileUploadService;
using NSubstitute;

namespace TestProject.Graph.Service.FileUploadServiceTests;

public class UploadFileServiceTests
{
    private readonly IFileUploadedRepository _fileUploadedRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly UploadFileService _sut;

    public UploadFileServiceTests()
    {
        _fileUploadedRepository = Substitute.For<IFileUploadedRepository>();
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _sut = new UploadFileService(_fileUploadedRepository, _categoryRepository);
    }

    [Fact]
    public async Task AddFileToDb_ShouldAddFileToRepository_WhenCalled()
    {
        // Arrange
        var categoryId = 1;
        var userId = Guid.NewGuid();
        var fileName = "TestFile.txt";
        var category = new Category(); 
        
        _categoryRepository.GetByIdAsync(categoryId).Returns(Task.FromResult(category));

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
    public async Task AddFileToDb_ShouldThrowCategoryResultNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = 1;
        var userId = Guid.NewGuid();
        var fileName = "TestFile.txt";
        
        _categoryRepository.GetByIdAsync(categoryId).Returns(Task.FromResult<Category>(null));

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("id", userId.ToString())
        }));

        // Act & Assert
        await Assert.ThrowsAsync<CategoryResultNotFoundException>(() => _sut.AddFileToDb(categoryId, claims, fileName));
    }

    [Fact]
    public async Task AddFileToDb_ShouldThrowFormatException_WhenInvalidUserId()
    {
        // Arrange
        var categoryId = 1;
        var invalidUserId = "invalid-guid";
        var fileName = "TestFile.txt";
        var category = new Category();

        _categoryRepository.GetByIdAsync(categoryId).Returns(Task.FromResult(category));

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("id", invalidUserId)
        }));

        // Act & Assert
        await Assert.ThrowsAsync<FormatException>(() => _sut.AddFileToDb(categoryId, claims, fileName));
    }
}