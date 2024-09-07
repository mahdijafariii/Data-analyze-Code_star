
using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.UserService.Abstraction;
using AnalysisData.User.Services.UserService.Business.Abstraction;
using AnalysisData.User.Services.ValidationService.Abstraction;

namespace AnalysisData.User.Services.UserService.Business;

public class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordService _passwordService;
    private readonly IValidationService _validationService;
    
    public PasswordManager(IPasswordHasher passwordHasher, IPasswordService passwordService, IValidationService validationService)
    {
        _passwordHasher = passwordHasher;
        _passwordService = passwordService;
        _validationService = validationService;
    }

    public async Task ResetPasswordAsync(Model.User user, string password, string confirmPassword)
    {
        _passwordService.ValidatePasswordAndConfirmation(password, confirmPassword);
        _validationService.PasswordCheck(password);
        user.Password = _passwordHasher.HashPassword(password);
    }

    public async Task NewPasswordAsync(Model.User user, string oldPassword, string password, string confirmPassword)
    {
        _passwordService.ValidatePassword(user, oldPassword);
        _passwordService.ValidatePasswordAndConfirmation(password, confirmPassword);
        user.Password = _passwordService.HashPassword(password);
    }
}