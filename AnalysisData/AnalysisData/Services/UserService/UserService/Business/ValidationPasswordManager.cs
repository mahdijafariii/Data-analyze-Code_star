using AnalysisData.Exception.PasswordException;
using AnalysisData.Models.UserModel;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;
using AnalysisData.Services.ValidationService.Abstraction;

namespace AnalysisData.Services.UserService.UserService.Business;

public class ValidationPasswordManager : IValidtionPasswordManager
{
    private readonly IPasswordHasherManager _passwordHasherManager;
    private readonly IValidationService _validationService;

    public ValidationPasswordManager(IPasswordHasherManager passwordHasherManager, IValidationService validationService)
    {
        _passwordHasherManager = passwordHasherManager ?? throw new ArgumentNullException(nameof(passwordHasherManager));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }

    public void ValidatePassword(User user, string password)
    {
        if (user.Password != _passwordHasherManager.HashPassword(password))
        {
            throw new InvalidPasswordException();
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
        return _passwordHasherManager.HashPassword(password);
    }
}