using AnalysisData.Exception.UserException;
using AnalysisData.User.Model;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.TokenService.Abstraction;
using AnalysisData.User.Services.ValidationService.Abstraction;


public class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordService _passwordService;
    private readonly IValidationService _validationService;
    private readonly IValidateTokenService _validateTokenService;
    private readonly IUserRepository _userRepository;

    public PasswordManager(IPasswordHasher passwordHasher, IPasswordService passwordService,
        IValidationService validationService, IValidateTokenService validateTokenService,
        IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _passwordService = passwordService;
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
        user.Password = _passwordHasher.HashPassword(password);
        await _userRepository.UpdateUserAsync(user.Id, user);
    }

    public async Task NewPasswordAsync(User user, string oldPassword, string password, string confirmPassword)
    {
        _passwordService.ValidatePassword(user, oldPassword);
        _passwordService.ValidatePasswordAndConfirmation(password, confirmPassword);
        user.Password = _passwordService.HashPassword(password);
    }
}