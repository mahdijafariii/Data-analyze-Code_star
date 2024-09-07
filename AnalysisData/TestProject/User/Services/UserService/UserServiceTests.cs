using System.Security.Claims;
using AnalysisData.User.CookieService.abstractions;
using AnalysisData.User.JwtService.abstractions;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.TokenService.Abstraction;
using AnalysisData.User.Services.UserService.Business.Abstraction;
using AnalysisData.User.Services.ValidationService.Abstraction;
using AnalysisData.User.UserDto.UserDto;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace TestProject.User.Services.UserService;

public class UserServiceTests
{
    private readonly IUserManager _userManager;
    private readonly IPasswordManager _passwordManager;
    private readonly ILoginManager _loginManager;
    private readonly IUserRepository _userRepository;
    private readonly IValidationService _validationService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidateTokenService _validateTokenService;
    private readonly AnalysisData.User.Services.UserService.UserService _sut;

    public UserServiceTests()
    {
        _userManager = Substitute.For<IUserManager>();
        _passwordManager = Substitute.For<IPasswordManager>();
        _loginManager = Substitute.For<ILoginManager>();
        _userRepository = Substitute.For<IUserRepository>();
        _validationService = Substitute.For<IValidationService>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _validateTokenService = Substitute.For<IValidateTokenService>();

        _sut = new AnalysisData.User.Services.UserService.UserService(_userRepository,
            _validationService, _passwordHasher, _validateTokenService, _userManager, _passwordManager, _loginManager);
    }
    //
    // [Fact]
    // public async Task ResetPasswordAsync_ShouldReturnTrue_WhenPasswordResetSucceeds()
    // {
    //     // Arrange
    //     var userClaim = new ClaimsPrincipal();
    //     var user = new AnalysisData.User.Model.User();
    //     _userManager.GetUserFromUserClaimsAsync(userClaim).Returns(Task.FromResult(user));
    //     _passwordManager.ResetPasswordAsync(user, "password", "password")
    //         .Returns(Task.FromResult(true));
    //
    //     // Act
    //     await _sut.ResetPasswordAsync(userClaim, "password", "password");
    //
    //     // Assert
    //     await _userManager.Received(1).GetUserFromUserClaimsAsync(userClaim);
    //     await _passwordManager.Received(1).ResetPasswordAsync(user, "password", "password");
    // }

    [Fact]
    public async Task NewPasswordAsync_ShouldReturnTrue_WhenPasswordChangeSucceeds()
    {
        // Arrange
        var userClaim = new ClaimsPrincipal();
        var user = new AnalysisData.User.Model.User();
        _userManager.GetUserFromUserClaimsAsync(userClaim).Returns(Task.FromResult(user));
        _passwordManager.NewPasswordAsync(user, "oldPassword", "newPassword", "newPassword")
            .Returns(Task.FromResult(true));

        // Act
        await _sut.NewPasswordAsync(userClaim, "oldPassword", "newPassword", "newPassword");

        // Assert
        await _userManager.Received(1).GetUserFromUserClaimsAsync(userClaim);
        await _passwordManager.Received(1).NewPasswordAsync(user, "oldPassword", "newPassword", "newPassword");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnUser_WhenLoginSucceeds()
    {
        // Arrange
        var userLoginDto = new UserLoginDto { UserName = "test", Password = "password" };
        var user = new AnalysisData.User.Model.User();
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
        var user = new AnalysisData.User.Model.User();
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
        var user = new AnalysisData.User.Model.User();
        _userManager.GetUserFromUserClaimsAsync(userClaim).Returns(Task.FromResult(user));
        _userManager.UpdateUserInformationAsync(user, updateUserDto).Returns(Task.FromResult(true));

        // Act
        await _sut.UpdateUserInformationAsync(userClaim, updateUserDto);

        // Assert
        await _userManager.Received(1).GetUserFromUserClaimsAsync(userClaim);
        await _userManager.Received(1).UpdateUserInformationAsync(user, updateUserDto);
    }
}