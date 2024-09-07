﻿using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.UserService.Business;
using AnalysisData.User.Services.UserService.Business.Abstraction;
using AnalysisData.User.Services.ValidationService.Abstraction;
using NSubstitute;

namespace TestProject.User.Services.UserService.BuisinessTests;

public class PasswordManagerTests
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordService _passwordService;
    private readonly IValidationService _validationService;
    private readonly PasswordManager _sut;

    public PasswordManagerTests()
    {
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _passwordService = Substitute.For<IPasswordService>();
        _validationService = Substitute.For<IValidationService>();
        _sut = new PasswordManager(_passwordHasher, _passwordService, _validationService);
    }
    
    [Fact]
    public async Task ResetPasswordAsync_ShouldCallValidatePasswordAndConfirmation_WhenCalled()
    {
        // Arrange
        var user = new AnalysisData.User.Model.User();
        var password = "newPassword";
        var confirmPassword = "newPassword";
        var token = "321231";
    
        // Act
        await _sut.ResetPasswordAsync(user, password, confirmPassword,token);
    
        // Assert
        _passwordService.Received().ValidatePasswordAndConfirmation(password, confirmPassword);
    }
    
    [Fact]
    public async Task ResetPasswordAsync_ShouldCallPasswordCheck_WhenCalled()
    {
        // Arrange
        var user = new AnalysisData.User.Model.User();
        var password = "newPassword";
        var confirmPassword = "newPassword";
        var token = "321231";

    
        // Act
        await _sut.ResetPasswordAsync(user, password, confirmPassword,token);
    
        // Assert
        _validationService.Received().PasswordCheck(password);
    }
    
    [Fact]
    public async Task ResetPasswordAsync_ShouldCallHashPasswordAndSetUserPassword_WhenCalled()
    {
        // Arrange
        var user = new AnalysisData.User.Model.User();
        var password = "newPassword";
        var hashedPassword = "hashedPassword";
        var confirmPassword = "newPassword";
    
        _passwordHasher.HashPassword(password).Returns(hashedPassword);
    
        // Act
        await _sut.ResetPasswordAsync(user, password, confirmPassword);
    
        // Assert
        Assert.Equal(hashedPassword, user.Password);
        _passwordHasher.Received().HashPassword(password);
    }

    [Fact]
    public async Task NewPasswordAsync_ShouldCallValidatePassword_WhenCalled()
    {
        // Arrange
        var user = new AnalysisData.User.Model.User();
        var oldPassword = "oldPassword";
        var password = "newPassword";
        var confirmPassword = "newPassword";

        // Act
        await _sut.NewPasswordAsync(user, oldPassword, password, confirmPassword);

        // Assert
        _passwordService.Received().ValidatePassword(user, oldPassword);
    }

    [Fact]
    public async Task NewPasswordAsync_ShouldCallValidatePasswordAndConfirmation_WhenCalled()
    {
        // Arrange
        var user = new AnalysisData.User.Model.User();
        var oldPassword = "oldPassword";
        var password = "newPassword";
        var confirmPassword = "newPassword";

        // Act
        await _sut.NewPasswordAsync(user, oldPassword, password, confirmPassword);

        // Assert
        _passwordService.Received().ValidatePasswordAndConfirmation(password, confirmPassword);
    }
}