using AnalysisData.Exception.UserException;
using AnalysisData.Models.GraphModel.Category;
using AnalysisData.Models.GraphModel.File;
using AnalysisData.Repositories.GraphRepositories.FileUploadedRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.UserFileRepository.Abstraction;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.GraphService.FilePermissionService.AccessManagement.Abstractions;
using Moq;

namespace TestProject.Graph.Service.FilePermissionService;

public class FilePermissionServiceTest
{
    private readonly Mock<IFileUploadedRepository> _fileUploadedRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUserFileRepository> _userFileRepositoryMock;
    private readonly Mock<IAccessManagementService> _accessManagementServiceMock;
    private readonly AnalysisData.Services.GraphService.FilePermissionService.FilePermissionService _sut;

    public FilePermissionServiceTest()
    {
        _fileUploadedRepositoryMock = new Mock<IFileUploadedRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userFileRepositoryMock = new Mock<IUserFileRepository>();
        _accessManagementServiceMock = new Mock<IAccessManagementService>();
        _fileUploadedRepositoryMock = new Mock<IFileUploadedRepository>();
        
        _sut = new AnalysisData.Services.GraphService.FilePermissionService.FilePermissionService(
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
    
    [Fact]
    public async Task GetUserForAccessingFileAsync_ShouldReturnUsers_WhenUsersExist()
    {
        // Arrange
        int page = 1;
        int limit = 2;
        string username = "mahdi";

        var mockUsers = new List<AnalysisData.Models.UserModel.User>()
        {
            new AnalysisData.Models.UserModel.User {Id = Guid.NewGuid(), Username = "mahdijm", FirstName ="mahdi", LastName = "jafari"},
            new AnalysisData.Models.UserModel.User {Id = Guid.NewGuid(), Username = "mahdijasem", FirstName ="mahdi", LastName = "jasemi"},
        };

        _userRepositoryMock.Setup(repo => repo.GetTopUsersByUsernameSearchAsync(username))
            .ReturnsAsync(mockUsers);
        
        // Act
        var result = await _sut.GetUserForAccessingFileAsync(username);

        // Assert
        Assert.Equal(mockUsers.Count,result.Count);
    }
    
    [Fact]
    public async Task GetUserForAccessingFileAsync_ShouldReturnUserNotFoundException_WhenNoUsersExist()
    {
        // Arrange
        string username = "temp";

        _userRepositoryMock.Setup(repo => repo.GetTopUsersByUsernameSearchAsync(username))
            .Throws<UserNotFoundException>();
        
        // Act && Assert
        Assert.ThrowsAsync<UserNotFoundException>(() => _sut.GetUserForAccessingFileAsync(username));
    }
    
    [Fact]
    public async Task WhoAccessThisFileAsync_ShouldReturnUsers_WhenUsersExistForFile()
    {
        // Arrange
        int fileId = 1;

        var userGuid1 = Guid.NewGuid();
        var userGuid2 = Guid.NewGuid();
        
        var mockUserFiles = new List<UserFile>
        {
            new UserFile { User = new AnalysisData.Models.UserModel.User { Id = userGuid1, Username = "user1" } },
            new UserFile { User = new AnalysisData.Models.UserModel.User { Id = userGuid2, Username = "user2" } }
        };

        _userFileRepositoryMock.Setup(repo => repo.GetByFileIdAsync(1))
            .ReturnsAsync(mockUserFiles);
        
        // Act
        var result = await _sut.WhoAccessThisFileAsync(fileId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        var resultList = result.ToList();
        Assert.Equal(userGuid1, resultList[0].Id);  
        Assert.Equal(userGuid2, resultList[1].Id);
    }

    [Fact] public async Task WhoAccessThisFileAsync_ShouldReturnEmptyList_WhenUsersNotExistForFile()
    {
        // Arrange
        int fileId = 1;
        var mockUserFiles = new List<UserFile>();

        _userFileRepositoryMock.Setup(repo => repo.GetByFileIdAsync(1))
            .ReturnsAsync(mockUserFiles);
        
        // Act
        var result = await _sut.WhoAccessThisFileAsync(fileId);

        // Assert
        Assert.Equal(0, result.Count());
    }
    
    
    [Fact]
    public async Task AccessFileToUserAsync_ShouldAccessFilesToInputUsers_WhenFileAndUserExist()
    {
        // Arrange
        var fileId = 1;
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var inputUserIds = new List<string> { guid1.ToString(), guid2.ToString() };

        var validGuids = new List<Guid>
        {
            guid1,
            guid2
        };

        var currentAccessor = new List<string> { "guid3", "guid4" }; 
        
        _fileUploadedRepositoryMock.Setup(repo => repo.GetByIdAsync(fileId))
            .ReturnsAsync(new FileEntity()); 

        _userFileRepositoryMock.Setup(repo => repo.GetUserIdsAccessHasToFile(fileId))
            .ReturnsAsync(currentAccessor);

        _accessManagementServiceMock.Setup(service => service.RevokeUserAccessAsync(It.IsAny<List<string>>()))
            .Returns(Task.CompletedTask);

        _accessManagementServiceMock.Setup(service => service.GrantUserAccessAsync(It.IsAny<List<string>>(), fileId))
            .Returns(Task.CompletedTask);

        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new AnalysisData.Models.UserModel.User()); 

        // Act
        await _sut.AccessFileToUserAsync(inputUserIds, fileId);

        // Assert
        var newUsers = validGuids.Select(g => g.ToString()).Except(currentAccessor).ToList();
        var blockAccessToFile = currentAccessor
            .Except(currentAccessor.Intersect(validGuids.Select(g => g.ToString()))).ToList();

        _accessManagementServiceMock.Verify(service => service.RevokeUserAccessAsync(blockAccessToFile), Times.Once);
        _accessManagementServiceMock.Verify(service => service.GrantUserAccessAsync(newUsers, fileId), Times.Once);
    }

    
    
}