using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.TokenService.Abstraction;
using AnalysisData.Services.UserService.UserService;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;
using AnalysisData.Services.ValidationService.Abstraction;
using NSubstitute;

namespace TestProject.Services.UserService.UserSerivce.BuisinessTests;

public class PasswordManagerTests
{
    private readonly IPasswordHasherManager _passwordHasherManager;
    private readonly IValidtionPasswordManager _validtionPasswordManager;
    private readonly IValidationService _validationService;
    private readonly IValidateTokenService _validateTokenService;
    private readonly IUserRepository _userRepository;
    private readonly PasswordService _sut;

    public PasswordManagerTests()
    {
        _passwordHasherManager = Substitute.For<IPasswordHasherManager>();
        _validtionPasswordManager = Substitute.For<IValidtionPasswordManager>();
        _validationService = Substitute.For<IValidationService>();
        _validateTokenService = Substitute.For<IValidateTokenService>();
        _userRepository = Substitute.For<IUserRepository>();
        _sut = new PasswordService(_passwordHasherManager, _validtionPasswordManager, _validationService,_validateTokenService,_userRepository);
    }

    [Fact]
    public async Task ResetPasswordAsync_ShouldCallPasswordCheck_WhenCalled()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new AnalysisData.Models.UserModel.User(){Id = userId, Password = "asd"};
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
        var user = new AnalysisData.Models.UserModel.User();
        var password = "newPassword";
        var hashedPassword = "hashedPassword";
        var confirmPassword = "newPassword";
        var token = "321231";

    
        _passwordHasherManager.HashPassword(password).Returns(hashedPassword);
    
        // Act
        await _sut.ResetPasswordAsync(user, password, confirmPassword,token);
    
        // Assert
        Assert.Equal(hashedPassword, user.Password);
        _passwordHasherManager.Received().HashPassword(password);
    }

    [Fact]
    public async Task NewPasswordAsync_ShouldCallValidatePassword_WhenCalled()
    {
        // Arrange
        var user = new AnalysisData.Models.UserModel.User();
        var oldPassword = "oldPassword";
        var password = "newPassword";
        var confirmPassword = "newPassword";

        // Act
        await _sut.NewPasswordAsync(user, oldPassword, password, confirmPassword);

        // Assert
        _validtionPasswordManager.Received().ValidatePassword(user, oldPassword);
    }

    [Fact]
    public async Task NewPasswordAsync_ShouldCallValidatePasswordAndConfirmation_WhenCalled()
    {
        // Arrange
        var user = new AnalysisData.Models.UserModel.User();
        var oldPassword = "oldPassword";
        var password = "newPassword";
        var confirmPassword = "newPassword";

        // Act
        await _sut.NewPasswordAsync(user, oldPassword, password, confirmPassword);

        // Assert
        _validtionPasswordManager.Received().ValidatePasswordAndConfirmation(password, confirmPassword);
    }
}