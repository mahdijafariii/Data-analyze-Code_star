using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.FileUploadedRepository;
using AnalysisData.EAV.Service;
using AnalysisData.Repository.UserRepository.Abstraction;
using Moq;

namespace TestProject.mahdiTest2.FilePermissionService;

public class FilePermissionServiceTest
{
    private readonly Mock<IFileUploadedRepository> _fileUploadedRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserFileRepository> _userFileRepositoryMock;
    private readonly Mock<IAccessManagementService> _accessManagementServiceMock;
    private readonly AnalysisData.EAV.Service.FilePermissionService _sut;

    public FilePermissionServiceTest()
    {
        _fileUploadedRepositoryMock = new Mock<IFileUploadedRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userFileRepositoryMock = new Mock<IUserFileRepository>();
        _accessManagementServiceMock = new Mock<IAccessManagementService>();
        _fileUploadedRepositoryMock = new Mock<IFileUploadedRepository>();
        
        _sut = new AnalysisData.EAV.Service.FilePermissionService(
            _fileUploadedRepositoryMock.Object,
            _userRepositoryMock.Object,
            _userFileRepositoryMock.Object,
            _accessManagementServiceMock.Object
            );
    }

    [Fact]
    public async Task GetFilesAsync_ShouldReturnPaginatedFiles_WhenFilesExist()
    {
        // Arrange
        int page = 1;
        int limit = 2;

        var mockFiles = new List<FileEntity>
        {
            new FileEntity { Id = 1, FileName = "file1.txt", Category = new Category { Name = "Documents" }, UploadDate = DateTime.UtcNow },
            new FileEntity { Id = 2, FileName = "file2.txt", Category = new Category { Name = "Images" }, UploadDate = DateTime.UtcNow }
        };

        _fileUploadedRepositoryMock.Setup(repo => repo.GetUploadedFilesAsync(page, limit))
            .ReturnsAsync(mockFiles);

        _fileUploadedRepositoryMock.Setup(repo => repo.GetTotalFilesCountAsync())
            .ReturnsAsync(5);

        // Act
        var result = await _sut.GetFilesAsync(page, limit);

        // Assert
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(5, result.TotalCount); 
        Assert.Equal(page, result.PageIndex);
        Assert.Contains(result.Items, f => f.FileName == "file1.txt");
        Assert.Contains(result.Items, f => f.FileName == "file2.txt");
    }
    
    [Fact]
    public async Task GetFilesAsync_ShouldReturnPaginatedFiles_WhenNoFilesExist()
    {
        // Arrange
        int page = 1;
        int limit = 2;

        var mockFiles = new List<FileEntity>();

        _fileUploadedRepositoryMock.Setup(repo => repo.GetUploadedFilesAsync(page, limit))
            .ReturnsAsync(mockFiles);

        _fileUploadedRepositoryMock.Setup(repo => repo.GetTotalFilesCountAsync())
            .ReturnsAsync(0);

        // Act
        var result = await _sut.GetFilesAsync(page, limit);

        // Assert
        Assert.Equal(0, result.Items.Count);
        Assert.Equal(0, result.TotalCount); 
        Assert.Equal(page, result.PageIndex);
        Assert.Equal(mockFiles.Count,result.Items.Count);
    }
    
    
    
}