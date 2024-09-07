using AnalysisData.Exception.PasswordException;
using AnalysisData.Models.UserModel;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.TokenService.Abstraction;
using AnalysisData.Services.UserService.UserService.Abstraction;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;
using AnalysisData.Services.ValidationService.Abstraction;

namespace AnalysisData.Services.UserService.UserService;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasherManager _passwordHasherManager;
    private readonly IValidtionPasswordManager _validtionPasswordManager;
    private readonly IValidationService _validationService;
    private readonly IValidateTokenService _validateTokenService;
    private readonly IUserRepository _userRepository;

    public PasswordService(IPasswordHasherManager passwordHasherManager, IValidtionPasswordManager validtionPasswordManager,
        IValidationService validationService, IValidateTokenService validateTokenService,
        IUserRepository userRepository)
    {
        _passwordHasherManager = passwordHasherManager;
        _validtionPasswordManager = validtionPasswordManager;
        _validationService = validationService;
        _validateTokenService = validateTokenService;
        _userRepository = userRepository;
    }

    public async Task ResetPasswordAsync(User user, string password, string confirmPassword,
        string resetPasswordToken)
    {
        await _validateTokenService.ValidateResetToken(user.Id, resetPasswordToken);
        if (password != confirmPassword)
        {
            throw new PasswordMismatchException();
        }
        _validationService.PasswordCheck(password);
        user.Password = _passwordHasherManager.HashPassword(password);
        await _userRepository.UpdateUserAsync(user.Id, user);
    }

    public async Task NewPasswordAsync(User user, string oldPassword, string password, string confirmPassword)
    {
        _validtionPasswordManager.ValidatePassword(user, oldPassword);
        _validtionPasswordManager.ValidatePasswordAndConfirmation(password, confirmPassword);
        user.Password = _validtionPasswordManager.HashPassword(password);
    }
}