using AnalysisData.Exception.UserException;
using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.UserService.Abstraction;
using AnalysisData.User.Services.UserService.Business.Abstraction;
using AnalysisData.User.Services.ValidationService.Abstraction;

namespace AnalysisData.User.Services.UserService.Business;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidationService _validationService;

    public PasswordService(IPasswordHasher passwordHasher, IValidationService validationService)
    {
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }

    public void ValidatePassword(Model.User user, string password)
    {
        if (user.Password != _passwordHasher.HashPassword(password))
        {
            throw new PasswordMismatchException();
        }
    }

    public void ValidatePasswordAndConfirmation(string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            throw new PasswordMismatchException();
        }
    }

    public string HashPassword(string password)
    {
        _validationService.PasswordCheck(password);
        return _passwordHasher.HashPassword(password);
    }
}