using AnalysisData.Exception;
using AnalysisData.Services.Abstraction;
using AnalysisData.Services.Business;
using AnalysisData.Services.Business.Abstraction;
using AnalysisData.Services.SecurityPasswordService.Abstraction;
using NSubstitute;

namespace TestProject.User.Services.Buisiness;

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
    public async Task ResetPasswordAsync_ValidInput_ShouldReturnTrue()
    {
        // Arrange
        var password = "amir123@";
        var comfirmPassword = "amir123@";
        var user = new AnalysisData.UserManage.Model.User();
        _passwordService.ValidatePasswordAndConfirmation(password, comfirmPassword);
        _validationService.PasswordCheck(password);
        _passwordHasher.HashPassword(password).Returns("HashedPassword");
        
        // Act
        var result = await _sut.ResetPasswordAsync(user, password, comfirmPassword);
        
        // Assert
        Assert.True(result);
        Assert.Equal("HashedPassword", user.Password);
    }

    [Fact]
    public async Task NewPasswordAsync_ValidInput_ShouldReturnTrue()
    {
        var oldPassword = "HashedPassword";
        var newPassword = "NewPassword";
        var ConfirmNewPassword = "NewPassword";
        
        var user = new AnalysisData.UserManage.Model.User
        {
            Password = "HashedPassword",
        };

        _passwordService.ValidatePassword(user, oldPassword);
        _passwordService.ValidatePasswordAndConfirmation(newPassword, ConfirmNewPassword);
        _passwordService.HashPassword(newPassword).Returns("NewHashedPassword");
        
        // Act
        var result = await _sut.NewPasswordAsync(user, oldPassword, newPassword, ConfirmNewPassword);

        Assert.True(result);
        Assert.Equal("NewHashedPassword", user.Password);

    }
    
    [Fact]
    public async Task ResetPasswordAsync_PasswordsDoNotMatch_ShouldThrowException()
    {
        // Arrange
        var user = new AnalysisData.UserManage.Model.User();
        var password = "Password123";
        var confirmPassword = "Password123!";
        
        _passwordService.When(x => x.ValidatePasswordAndConfirmation(password, confirmPassword))
            .Do(x => throw new PasswordMismatchException());

        // Act & Assert
        await Assert.ThrowsAsync<PasswordMismatchException>(() => _sut.ResetPasswordAsync(user, password, confirmPassword));


    }
    
    [Fact]
    public async Task NewPasswordAsync_InvalidOldPassword_ShouldThrowException()
    {
        // Arrange
        var user = new AnalysisData.UserManage.Model.User { Password = "OldHashedPassword" };
        var oldPassword = "InvalidOldPassword";
        var newPassword = "NewPassword123!";
        var confirmPassword = "NewPassword123!";

        _passwordService.When(x => x.ValidatePassword(user, oldPassword))
            .Do(x => throw new PasswordMismatchException());

        // Act & Assert
        await Assert.ThrowsAsync<PasswordMismatchException>(() => _sut.NewPasswordAsync(user, oldPassword, newPassword, confirmPassword));
    }
    
}