using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Exception.PasswordException;
using AnalysisData.Exception.UserException;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.CookieService.Abstractions;
using AnalysisData.Services.JwtService.Abstraction;
using AnalysisData.Services.UserService.UserService.Business;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;
using NSubstitute;

namespace TestProject.User.Services.UserService.BuisinessTests;

public class LoginManagerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IValidtionPasswordManager _validtionPasswordManager;
    private readonly IJwtService _jwtService;
    private readonly ICookieService _cookieService;
    private readonly LoginManager _sut;

    public LoginManagerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _validtionPasswordManager = Substitute.For<IValidtionPasswordManager>();
        _jwtService = Substitute.For<IJwtService>();
        _cookieService = Substitute.For<ICookieService>();
        _sut = new LoginManager(_userRepository, _validtionPasswordManager, _jwtService, _cookieService);
    }
    
    [Fact]
    public async Task LoginAsync_ShouldReturnUserAndSetCookie_WhenValidCredentialsProvided()
    {
        // Arrange
        var token = "generatedJwtToken";
        var userLoginDto = new UserLoginDto
        {
            UserName = "testUser",
            Password = "ValidPassword123!",
            RememberMe = true
        };
        var user = new AnalysisData.Models.UserModel.User()
        {
            Username = "testUser",
            Password = "ValidPassword123",
        };

        _userRepository.GetUserByUsernameAsync(userLoginDto.UserName).Returns(user);
        _validtionPasswordManager.ValidatePassword(user, userLoginDto.Password);
        _jwtService.GenerateJwtToken(userLoginDto.UserName).Returns(token);

        // Act
        var result = await _sut.LoginAsync(userLoginDto);

        // Assert
        Assert.Equal(user, result);
        _cookieService.Received(1).SetCookie("AuthToken", token, userLoginDto.RememberMe);
    }
    
    [Fact]
    public async Task LoginAsync_ShouldThrowUserNotFoundException_WhenInvalidUsernameProvided()
    {
        // Arrange
        var userLoginDto = new UserLoginDto
        {
            UserName = "invalidUser",
            Password = "SomePassword123!",
            RememberMe = false
        };

        _userRepository.GetUserByUsernameAsync(userLoginDto.UserName)
            .Returns(Task.FromResult<AnalysisData.Models.UserModel.User>(null));

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _sut.LoginAsync(userLoginDto));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrowPasswordMismatchException_WhenInvalidPasswordProvided()
    {
        // Arrange
        var userLoginDto = new UserLoginDto
        {
            UserName = "testUser",
            Password = "InvalidPassword123!",
            RememberMe = false
        };
        var user = new AnalysisData.Models.UserModel.User() { Username = "testUser", Password = "HashedPassword" };

        _userRepository.GetUserByUsernameAsync(userLoginDto.UserName).Returns(Task.FromResult(user));
        _validtionPasswordManager.When(x => x.ValidatePassword(user, userLoginDto.Password))
            .Do(x => throw new PasswordMismatchException());

        // Act & Assert
        await Assert.ThrowsAsync<PasswordMismatchException>(() => _sut.LoginAsync(userLoginDto));
    }
}