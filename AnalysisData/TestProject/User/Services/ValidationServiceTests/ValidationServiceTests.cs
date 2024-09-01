using AnalysisData.Exception;

namespace TestProject.User.Services.ValidationService;

public class ValidationServiceTests
{
    private readonly AnalysisData.Services.ValidationService _validationService;

    public ValidationServiceTests()
    {
        _validationService = new AnalysisData.Services.ValidationService();    
    }
    
    
        [Theory]
        [InlineData("user@example.com")]
        [InlineData("user.name@example.co.uk")]
        [InlineData("user_name+test@example.io")]
        public void EmailCheck_ValidEmail_ShouldNotThrowException(string email)
        {
            // Act & Assert
            _validationService.EmailCheck(email);
        }

        [Theory]
        [InlineData("plainaddress")]
        [InlineData("@missingusername.com")]
        [InlineData("user@.com")]
        [InlineData("user@domain..com")]
        public void EmailCheck_InvalidEmail_ShouldThrowInvalidEmailFormatException(string email)
        {
            // Act & Assert
            Assert.Throws<InvalidEmailFormatException>(() => _validationService.EmailCheck(email));
        }

        [Theory]
        [InlineData("09130000000")]
        [InlineData("09120000000")]
        public void PhoneNumberCheck_ValidPhoneNumber_ShouldNotThrowException(string phoneNumber)
        {
            // Act & Assert
            _validationService.PhoneNumberCheck(phoneNumber);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("phone123")]
        [InlineData("123-abc-7890")]
        [InlineData("(123) 456-78901")]
        public void PhoneNumberCheck_InvalidPhoneNumber_ShouldThrowInvalidPhoneNumberFormatException(string phoneNumber)
        {
            // Act & Assert
            Assert.Throws<InvalidPhoneNumberFormatException>(() => _validationService.PhoneNumberCheck(phoneNumber));
        }

        [Theory]
        [InlineData("Password123!")]
        [InlineData("StrongPassword@2023")]
        [InlineData("Valid1234$")]
        public void PasswordCheck_ValidPassword_ShouldNotThrowException(string password)
        {
            // Act & Assert
            _validationService.PasswordCheck(password);
        }

        [Theory]
        [InlineData("short")]
        [InlineData("123456")]
        [InlineData("password")]
        [InlineData("NoSpecialChar123")]
        public void PasswordCheck_InvalidPassword_ShouldThrowInvalidPasswordFormatException(string password)
        {
            // Act & Assert
            Assert.Throws<InvalidPasswordFormatException>(() => _validationService.PasswordCheck(password));
        }
    
    
}