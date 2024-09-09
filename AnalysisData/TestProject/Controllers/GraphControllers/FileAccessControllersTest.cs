using AnalysisData.Controllers.GraphController;
using AnalysisData.Dtos.GraphDto.FileDto;
using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Models.GraphModel.File;
using AnalysisData.Repositories.GraphRepositories.UserFileRepository.Abstraction;
using AnalysisData.Services.GraphService.FilePermissionService.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace TestProject.Controllers.GraphControllers;

public class FileAccessControllersTest
{
    private readonly FileAccessController _sut;
    private readonly Mock<IFilePermissionService> _filePermissionServiceMock;
    private readonly Mock<IUserFileRepository> _userFileRepositoryMock;

    public FileAccessControllersTest()
    {
        _filePermissionServiceMock = new Mock<IFilePermissionService>();
        _userFileRepositoryMock = new Mock<IUserFileRepository>();
        _sut = new FileAccessController(_filePermissionServiceMock.Object, _userFileRepositoryMock.Object);
    }
    [Fact]
    public async Task GetFilesAsync_ShouldReturnOkResult_WhenFilesAreFound()
    {
        // Arrange
        int page = 1;
        int limit = 10;
        var files = new List<FileEntityDto>
        {
            new FileEntityDto { Id = 1, FileName = "File1" },
            new FileEntityDto { Id = 2, FileName = "File2" }
        };
        PaginatedFileDto paginatedFiles = new PaginatedFileDto(files,30,page);
    
        _filePermissionServiceMock.Setup(service => service.GetFilesAsync(page, limit))
            .ReturnsAsync(paginatedFiles);

        // Act
        var result = await _sut.GetFilesAsync(page, limit);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(paginatedFiles);
        Assert.Equal(expectedResponseContent, responseContent);

        _filePermissionServiceMock.Verify(service => service.GetFilesAsync(page, limit), Times.Once);
    }
    [Fact]
    public async Task GetUsersAsync_ShouldReturnOkResult_WhenUsersAreFound()
    {
        // Arrange
        string username = "mahdi";
        var users = new List<UserAccessDto>
        {
            new UserAccessDto { Id = Guid.NewGuid().ToString(), UserName = "User1" },
            new UserAccessDto { Id = Guid.NewGuid().ToString(), UserName = "User2" }
        };

        _filePermissionServiceMock.Setup(service => service.GetUserForAccessingFileAsync(username))
            .ReturnsAsync(users);

        // Act
        var result = await _sut.GetUsersAsync(username);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(users);
        Assert.Equal(expectedResponseContent, responseContent);

        _filePermissionServiceMock.Verify(service => service.GetUserForAccessingFileAsync(username), Times.Once);
    }

    [Fact]
    public async Task AccessFileToUser_ShouldReturnOk_WhenAccessGrantedSuccessfully()
    {
        // Arrange
        var request = new AccessFileToUserDto
        {
            UserGuidIds = new List<string> { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() },
            FileId = 1
        };
    
        // Act
        var result = await _sut.AccessFileToUser(request);
    
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { massage = "success" });
        Assert.Equal(expectedResponseContent, responseContent);
    
        _filePermissionServiceMock.Verify(service => service.AccessFileToUserAsync(request.UserGuidIds.ToList(), request.FileId), Times.Once);
    }
    
    [Fact]
    public async Task WhoAccessToThisFile_ShouldReturnOkResult_WhenFileIsFound()
    {
        // Arrange
        int fileId = 1;
        var file = new List<WhoAccessThisFileDto>
        {
            new WhoAccessThisFileDto() { Id = Guid.NewGuid(), UserName = "User1" },
            new WhoAccessThisFileDto() { Id = Guid.NewGuid(), UserName = "User2" }
        };
        
        var userFile = new List<UserFile>
        {
            new UserFile() { Id = Guid.NewGuid(), User = new AnalysisData.Models.UserModel.User() },
            new UserFile() { Id = Guid.NewGuid(), User = new AnalysisData.Models.UserModel.User() }
        };
    
        _userFileRepositoryMock.Setup(repo => repo.GetByFileIdAsync(fileId))
            .ReturnsAsync(userFile);
    
        _filePermissionServiceMock.Setup(service => service.WhoAccessThisFileAsync(fileId))
            .ReturnsAsync(file);
    
        // Act
        var result = await _sut.WhoAccessToThisFile(fileId);
    
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(file);
        Assert.Equal(expectedResponseContent, responseContent);
    
        _filePermissionServiceMock.Verify(service => service.WhoAccessThisFileAsync(fileId), Times.Once);
    }
    
    [Fact]
    public async Task WhoAccessToThisFile_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        int fileId = 1;
    
        _userFileRepositoryMock.Setup(repo => repo.GetByFileIdAsync(fileId))
            .ReturnsAsync((List<UserFile>)null);
    
        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() => _sut.WhoAccessToThisFile(fileId));
    
        _filePermissionServiceMock.Verify(service => service.WhoAccessThisFileAsync(fileId), Times.Never);
    }
    
}
