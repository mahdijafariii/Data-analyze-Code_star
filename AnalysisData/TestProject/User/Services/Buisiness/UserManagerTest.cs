using System.Security.Claims;
using AnalysisData.Exception;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.Model; 
using AnalysisData.Services.Business;
using AnalysisData.UserManage.UpdateModel;
using NSubstitute;

namespace TestProject.User.Services.Buisiness;

public class UserManagerTest
{
    private readonly IUserRepository _userRepository;
    private readonly IValidationService _validationService;
    private readonly UserManager _sut;

    public UserManagerTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _validationService = Substitute.For<IValidationService>();
        _sut = new UserManager(_userRepository, _validationService);
    }

    [Fact]
    public async Task GetUserAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var userClaim = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("username", "testUser")
        }));
        var userId = Guid.NewGuid(); 
        var expectedUser = new AnalysisData.UserManage.Model.User { Id = userId , Username = "testUser" };
        _userRepository.GetUserByUsernameAsync("testUser").Returns(expectedUser);

        // Act
        var result = await _sut.GetUserAsync(userClaim);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser, result);
        Assert.Equal(expectedUser.Id, result.Id);
        Assert.Equal(expectedUser.Username, result.Username);
        Assert.Equal(expectedUser.Email, result.Email);
        Assert.Equal(expectedUser.FirstName, result.FirstName);
        Assert.Equal(expectedUser.LastName, result.LastName);
        Assert.Equal(expectedUser.PhoneNumber, result.PhoneNumber);
        Assert.Equal(expectedUser.ImageURL, result.ImageURL);
    }
    
    [Fact]
    public async Task GetUserAsync_UserDoesNotExist_ThrowsUserNotFoundException()
    {
        // Arrange
        var userClaim = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("username", "nonExistingUser")
        }));

        _userRepository.GetUserByUsernameAsync("nonExistingUser").Returns((AnalysisData.UserManage.Model.User)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _sut.GetUserAsync(userClaim));
    }
    [Fact]
    public async Task UpdateUserInformationAsync_ValidData_ReturnsTrue()
    {
        // Arrange
        var user = new AnalysisData.UserManage.Model.User { Id = Guid.NewGuid(), Email = "info@example.com", FirstName = "sdas", LastName = "asdsdqss"};
        var updateUserDto = new UpdateUserDto
        {
            FirstName = "NewFirstName",
            LastName = "NewLastName",
            Email = "info2@example.com",
            PhoneNumber = "09130000000"
        };

        _userRepository.GetUserByEmailAsync("info2@example.com").Returns((AnalysisData.UserManage.Model.User)null);

        // Act
        var result = await _sut.UpdateUserInformationAsync(user, updateUserDto);

        // Assert
        Assert.True(result);
        _userRepository.Received(1).UpdateUserAsync(user.Id, user);
        _validationService.Received(1).EmailCheck(updateUserDto.Email);
        _validationService.Received(1).PhoneNumberCheck(updateUserDto.PhoneNumber);
    }
    
    [Fact]
    public async Task UpdateUserInformationAsync_EmailAlreadyExists_ThrowsDuplicateUserException()
    {
        // Arrange
        var user = new AnalysisData.UserManage.Model.User { Id = Guid.NewGuid(), Email = "info@gmail.com" };
        var updateUserDto = new UpdateUserDto
        {
            FirstName = "NewFirstName",
            LastName = "NewLastName",
            Email = "info2@gmail.com",
            PhoneNumber = "1234567890"
        };

        var existingUser = new AnalysisData.UserManage.Model.User { Id = Guid.NewGuid(), Email = "info2@gmail.com" };

        _userRepository.GetUserByEmailAsync("info2@gmail.com").Returns(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateUserException>(() => _sut.UpdateUserInformationAsync(user, updateUserDto));
    }



}