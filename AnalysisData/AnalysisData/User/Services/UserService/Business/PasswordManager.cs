using AnalysisData.Services.Abstraction;
using AnalysisData.Services.Business.Abstraction;
using AnalysisData.Services.SecurityPasswordService.Abstraction;
using AnalysisData.UserManage.Model;
using Microsoft.AspNetCore.Identity;

namespace AnalysisData.Services.Business;

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

    public async Task<bool> ResetPasswordAsync(User user, string password, string confirmPassword)
    {
        _passwordService.ValidatePasswordAndConfirmation(password, confirmPassword);
        _validationService.PasswordCheck(password);
        user.Password = _passwordHasher.HashPassword(password);
        return true;
    }

    public async Task<bool> NewPasswordAsync(User user, string oldPassword, string password, string confirmPassword)
    {
        _passwordService.ValidatePassword(user, oldPassword);
        _passwordService.ValidatePasswordAndConfirmation(password, confirmPassword);
        user.Password = _passwordService.HashPassword(password);
        return true;
    }
}