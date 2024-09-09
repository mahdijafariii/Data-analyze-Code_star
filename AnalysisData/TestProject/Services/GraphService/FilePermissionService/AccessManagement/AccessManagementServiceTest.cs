using AnalysisData.Models.GraphModel.File;
using AnalysisData.Repositories.GraphRepositories.UserFileRepository.Abstraction;
using AnalysisData.Services.GraphService.FilePermissionService.AccessManagement;
using Moq;

public class AccessManagementServiceTest
{
    private readonly Mock<IUserFileRepository> _userFileRepositoryMock;
    private readonly AccessManagementService _sut;

    public AccessManagementServiceTest()
    {
        _userFileRepositoryMock = new Mock<IUserFileRepository>();
        _sut = new AccessManagementService(_userFileRepositoryMock.Object);
    }

    [Fact]
    public async Task GrantUserAccessAsync_ShouldAddUserFileEntries_WhenValidUserIdsAreProvided()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        var userIds = new List<string> { guid1.ToString(), guid2.ToString() };
        var fileId = 1;

        // Act
        await _sut.GrantUserAccessAsync(userIds, fileId);

        // Assert
        foreach (var userId in userIds)
        {
            _userFileRepositoryMock.Verify(repo => repo.AddAsync(It.Is<UserFile>(
                uf => uf.UserId == Guid.Parse(userId) && uf.FileId == fileId)), Times.Once);
        }
    }
    
    [Fact]
    public async Task RevokeUserAccessAsync_ShouldRemoveUserFileEntries_WhenValidUserIdsAreProvided()
    {
        // Arrange
        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();

        var userIds = new List<string> { guid1.ToString(), guid2.ToString() };

        // Act
        await _sut.RevokeUserAccessAsync(userIds);

        // Assert
        foreach (var userId in userIds)
        {
            _userFileRepositoryMock.Verify(repo => repo.DeleteByUserIdAsync(Guid.Parse(userId)), Times.Once);
        }
    }
    
}