﻿using AnalysisData.CookieService.abstractions;
using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Business;
using AnalysisData.Services.Business.Abstraction;
using AnalysisData.Services.SecurityPasswordService.Abstraction;
using AnalysisData.UserManage.LoginModel;
using NSubstitute;

namespace TestProject.User.Services.Buisiness;

public class LoginManagerTest
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly ICookieService _cookieService;
    private readonly LoginManager _sut;

    public LoginManagerTest()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordService = Substitute.For<IPasswordService>();
        _jwtService = Substitute.For<IJwtService>();
        _cookieService = Substitute.For<ICookieService>();
        _sut = new LoginManager(_userRepository, _passwordService, _jwtService, _cookieService);
    }
    
    [Fact]
    public async Task LoginAsync_ValidCredentials_ShouldReturnUserAndSetCookie()
    {
        // Arrange
        var token = "generatedJwtToken";
        var userLoginDto = new UserLoginDto
        {
            userName = "testUser",
            password = "ValidPassword123!",
            rememberMe = true
        };
        var user = new AnalysisData.UserManage.Model.User
        {
            Username = "testUser",
            Password = "ValidPassword123",
        };

        _userRepository.GetUserByUsernameAsync(userLoginDto.userName).Returns(user);
        _passwordService.ValidatePassword(user, userLoginDto.password);
        _jwtService.GenerateJwtToken(userLoginDto.userName).Returns(token);
        

        // Act
        var result = await _sut.LoginAsync(userLoginDto);
        
        Assert.Equal(user, result);
        _cookieService.Received(1).SetCookie("AuthToken", token, userLoginDto.rememberMe);
    }
    
    [Fact]
    public async Task LoginAsync_InvalidUsername_ShouldThrowUserNotFoundException()
    {
        // Arrange
        var userLoginDto = new UserLoginDto
        {
            userName = "invalidUser",
            password = "SomePassword123!",
            rememberMe = false
        };

        _userRepository.GetUserByUsernameAsync(userLoginDto.userName).Returns(Task.FromResult<AnalysisData.UserManage.Model.User>(null));

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _sut.LoginAsync(userLoginDto));
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ShouldThrowPasswordMismatchException()
    {
        // Arrange
        var userLoginDto = new UserLoginDto
        {
            userName = "testUser",
            password = "InvalidPassword123!",
            rememberMe = false
        };
        var user = new AnalysisData.UserManage.Model.User { Username = "testUser", Password = "HashedPassword" };

        _userRepository.GetUserByUsernameAsync(userLoginDto.userName).Returns(Task.FromResult(user));
        _passwordService.When(x => x.ValidatePassword(user, userLoginDto.password))
            .Do(x => throw new PasswordMismatchException());

        // Act & Assert
        await Assert.ThrowsAsync<PasswordMismatchException>(() => _sut.LoginAsync(userLoginDto));
    }
}