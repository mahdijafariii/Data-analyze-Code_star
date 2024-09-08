using System.Security.Claims;
using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Models.UserModel;
using AnalysisData.Services.UserService.UserService;
using AnalysisData.Services.UserService.UserService.Abstraction;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;
using NSubstitute;


public class UserServiceTests
{
    private readonly IUserManager _userManager;
    private readonly IPasswordService _passwordService;
    private readonly ILoginManager _loginManager;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _userManager = Substitute.For<IUserManager>();
        _passwordService = Substitute.For<IPasswordService>();
        _loginManager = Substitute.For<ILoginManager>();
        _sut = new UserService(_userManager, _passwordService, _loginManager);
    }

    [Fact]
    public async Task ResetPasswordAsync_ShouldReturnTrue_WhenPasswordResetSucceeds()
    {
        // Arrange
        var userClaim = new ClaimsPrincipal();
        var user = new User();
        _userManager.GetUserFromEmail("mahdijm.bb@gmail.com").Returns(Task.FromResult(user));
        var tokenGuid = Guid.NewGuid().ToString();
        _passwordService.ResetPasswordAsync(user, "password", "password", tokenGuid)
            .Returns(Task.FromResult(true));

        // Act
        await _sut.ResetPasswordAsync("mahdijm.bb@gmail.com", "password", "password", tokenGuid);

        // Assert
        await _userManager.Received(1).GetUserFromEmail("mahdijm.bb@gmail.com");
        await _passwordService.Received(1).ResetPasswordAsync(user, "password", "password", tokenGuid);
    }

    [Fact]
    public async Task NewPasswordAsync_ShouldReturnTrue_WhenPasswordChangeSucceeds()
    {
        // Arrange
        var userClaim = new ClaimsPrincipal();
        var user = new User();
        _userManager.GetUserFromUserClaimsAsync(userClaim).Returns(Task.FromResult(user));
        _passwordService.NewPasswordAsync(user, "oldPassword", "newPassword", "newPassword")
            .Returns(Task.FromResult(true));

        // Act
        await _sut.NewPasswordAsync(userClaim, "oldPassword", "newPassword", "newPassword");

        // Assert
        await _userManager.Received(1).GetUserFromUserClaimsAsync(userClaim);
        await _passwordService.Received(1).NewPasswordAsync(user, "oldPassword", "newPassword", "newPassword");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnUser_WhenLoginSucceeds()
    {
        // Arrange
        var userLoginDto = new UserLoginDto { UserName = "test", Password = "password" };
        var user = new User();
        _loginManager.LoginAsync(userLoginDto).Returns(Task.FromResult(user));

        // Act
        var result = await _sut.LoginAsync(userLoginDto);

        // Assert
        Assert.Equal(user, result);
        await _loginManager.Received(1).LoginAsync(userLoginDto);
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnUser_WhenUserIsFound()
    {
        // Arrange
        var userClaim = new ClaimsPrincipal();
        var user = new User();
        _userManager.GetUserFromUserClaimsAsync(userClaim).Returns(Task.FromResult(user));

        // Act
        var result = await _sut.GetUserAsync(userClaim);

        // Assert
        Assert.Equal(user, result);
        await _userManager.Received(1).GetUserFromUserClaimsAsync(userClaim);
    }

    [Fact]
    public async Task UpdateUserInformationAsync_ShouldReturnTrue_WhenUpdateSucceeds()
    {
        // Arrange
        var userClaim = new ClaimsPrincipal();
        var updateUserDto = new UpdateUserDto { FirstName = "John", LastName = "Doe" };
        var user = new User();
        _userManager.GetUserFromUserClaimsAsync(userClaim).Returns(Task.FromResult(user));
        _userManager.UpdateUserInformationAsync(user, updateUserDto).Returns(Task.FromResult(true));

        // Act
        await _sut.UpdateUserInformationAsync(userClaim, updateUserDto);

        // Assert
        await _userManager.Received(1).GetUserFromUserClaimsAsync(userClaim);
        await _userManager.Received(1).UpdateUserInformationAsync(user, updateUserDto);
    }
}