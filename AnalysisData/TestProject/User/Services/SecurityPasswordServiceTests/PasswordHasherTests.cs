using AnalysisData.Exception;
using AnalysisData.User.Services.SecurityPasswordService;
using Microsoft.AspNetCore.Identity;

namespace TestProject.User.Services.SecurityPasswordServiceTests;

public class PasswordHasherTests
{
    private readonly PasswordHasher _passwordHasher;

    public PasswordHasherTests()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword_WhenValidPasswordProvided()
    {
        // Arrange
        var password = "MySecurePassword123!";
        
        // Act
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
    }

    [Fact]
    public void HashPassword_ShouldReturnSameHash_WhenSamePasswordProvided()
    {
        // Arrange
        var password = "MySecurePassword123!";
        
        // Act
        var hashedPassword1 = _passwordHasher.HashPassword(password);
        var hashedPassword2 = _passwordHasher.HashPassword(password);

        // Assert
        Assert.Equal(hashedPassword1, hashedPassword2);
    }

    [Fact]
    public void HashPassword_ShouldReturnDifferentHashes_WhenDifferentPasswordsProvided()
    {
        // Arrange
        var password1 = "MySecurePassword123!";
        var password2 = "AnotherPassword456!";
        
        // Act
        var hashedPassword1 = _passwordHasher.HashPassword(password1);
        var hashedPassword2 = _passwordHasher.HashPassword(password2);

        // Assert
        Assert.NotEqual(hashedPassword1, hashedPassword2);
    }

    [Fact]
    public void HashPassword_ShouldReturnHash_WhenEmptyPasswordProvided()
    {
        // Arrange
        var password = string.Empty;
        
        // Act
        var hashedPassword = _passwordHasher.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
    }

    [Fact]
    public void HashPassword_ShouldThrowArgumentNullException_WhenNullPasswordProvided()
    {
        // Act & Assert
        Assert.Throws<PasswordHasherInputNull>(() => _passwordHasher.HashPassword(null));
    }
}