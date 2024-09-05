using AnalysisData.Exception;
using AnalysisData.Services.Abstraction;
using AnalysisData.Services.Business;
using AnalysisData.Services.SecurityPasswordService.Abstraction;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace TestProject.User.Services.Buisiness;

public class PasswordServiceTests
{
    private readonly IPasswordHasher _passwordHasherMock;
    private readonly IValidationService _validationServiceMock;
    private readonly PasswordService _sut;

    public PasswordServiceTests()
    {
        _passwordHasherMock = Substitute.For<IPasswordHasher>();
        _validationServiceMock = Substitute.For<IValidationService>();
        _sut = new PasswordService(_passwordHasherMock, _validationServiceMock);
    }

    [Fact]
    public void ValidatePassword_ValidPassword_DoesNotThrowException()
    {
        // Arrange
        var user = new AnalysisData.UserManage.Model.User
        {
            Password = "9883AF16067978706D06310A5BD58D0FF176BD738AC675DC49BF8F244666456A" 
        };

        var plainText = "lndkj";
        _passwordHasherMock
            .HashPassword(plainText)
            .Returns("9883AF16067978706D06310A5BD58D0FF176BD738AC675DC49BF8F244666456A");

        // Act
        _sut.ValidatePassword(user, plainText);

        // Assert
        // No exception
    }

    [Fact]
    public void ValidatePassword_ShouldThrowPasswordMismatchException_WhenInvalidPasswordProvided()
    {
        // Arrange
        var user = new AnalysisData.UserManage.Model.User
        {
            Password = "9883AF16067978706D06310A5BD58D0FF176BD738AC675DC49BF8F244666456A" 
        };

        var plainText = "plainText";

        _passwordHasherMock
            .HashPassword(plainText)
            .Returns("08A9B262078244CB990CB5746A7364BFD0B1EF95F8DD9AB61491EDC050A1EB7E");

        // Act
        Action act = () => _sut.ValidatePassword(user, plainText);

        // Assert
        Assert.Throws<PasswordMismatchException>(act);
    }
    
    [Fact]
    public void ValidatePasswordAndConfirmation_ShouldNotThrowException_WhenPasswordsMatch()
    {
        // Arrange
        var password = "password";
        var confirmPassword = "password";

        // Act & Assert
        _sut.ValidatePasswordAndConfirmation(password, confirmPassword);
    }
    
    [Fact]
    public void ValidatePasswordAndConfirmation_ShouldThrowPasswordMismatchException_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var password = "password";
        var confirmPassword = "differentPassword";

        // Act & Assert
        Assert.Throws<PasswordMismatchException>(() => _sut.ValidatePasswordAndConfirmation(password, confirmPassword));
    }
    
    [Fact]
    public void HashPassword_ShouldReturnHashedPassword_WhenValidPasswordProvided()
    {
        // Arrange
        var plainPassword = "ValidPass123!@#";
        var hashedPassword = "EC3D11401772029C7E66E1B96C6B63FA379D70C47B0C9724CB64D162D5BE7701";
        
        _validationServiceMock
            .When(service => service.PasswordCheck(plainPassword))
            .Do(call => { /* No exception */ });
        
        _passwordHasherMock
            .HashPassword(plainPassword)
            .Returns(hashedPassword);

        // Act
        var result = _sut.HashPassword(plainPassword);

        // Assert
        Assert.Equal(hashedPassword, result);
        _validationServiceMock.Received(1).PasswordCheck(plainPassword);
    }

    [Fact]
    public void HashPassword_ShouldThrowInvalidPasswordFormatException_WhenInvalidPasswordProvided()
    {
        // Arrange
        var invalidPassword = "short";
    
        _validationServiceMock
            .When(service => service.PasswordCheck(invalidPassword))
            .Do(call => throw new InvalidPasswordFormatException());

        // Act
        Func<string> act = () => _sut.HashPassword(invalidPassword);

        // Assert
        var exception = Assert.Throws<InvalidPasswordFormatException>(act);
        _validationServiceMock.Received(1).PasswordCheck(invalidPassword);
    }
}