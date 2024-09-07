using System.Security.Claims;
using AnalysisData.Exception.UserException;
using AnalysisData.User.CookieService.abstractions;
using AnalysisData.User.JwtService.abstractions;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.TokenService.Abstraction;
using AnalysisData.User.Services.UserService.Abstraction;
using AnalysisData.User.Services.UserService.Business.Abstraction;
using AnalysisData.User.Services.ValidationService.Abstraction;
using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserManager _userManager;
    private readonly IPasswordManager _passwordManager;
    private readonly ILoginManager _loginManager;
    private readonly IUserRepository _userRepository;

    private readonly IValidationService _validationService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidateTokenService _validateTokenService;


    public UserService(IUserRepository userRepository, IValidationService validationService, IPasswordHasher passwordHasher,IValidateTokenService validateTokenService,IUserManager userManager, IPasswordManager passwordManager, ILoginManager loginManager)
    {
        _userRepository = userRepository;
        _validationService = validationService;
        _passwordHasher = passwordHasher;
        _validateTokenService = validateTokenService;
        _userManager = userManager;
        _passwordManager = passwordManager;
        _loginManager = loginManager;
    }

    public async Task<bool> ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword , string resetPasswordToken)
    {
        var userName = userClaim.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        await _validateTokenService.ValidateResetToken(Guid.Parse(userName),resetPasswordToken);

        if (password != confirmPassword)
        {
            throw new PasswordMismatchException();
        }

        _validationService.PasswordCheck(password);
        user.Password = _passwordHasher.HashPassword(password);
        await _userRepository.UpdateUserAsync(user.Id, user);
        return true;
    }

    public async Task ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword)
    {
        var user = await _userManager.GetUserFromUserClaimsAsync(userClaim);
        await _passwordManager.ResetPasswordAsync(user, password, confirmPassword);
    }

    public async Task NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword)
    {
        var user = await _userManager.GetUserFromUserClaimsAsync(userClaim);
        await _passwordManager.NewPasswordAsync(user, oldPassword, password, confirmPassword);
    }

    public async Task<Model.User> LoginAsync(UserLoginDto userLoginDto)
    {
        return await _loginManager.LoginAsync(userLoginDto);
    }

    public async Task<Model.User> GetUserAsync(ClaimsPrincipal userClaim)
    {
        return await _userManager.GetUserFromUserClaimsAsync(userClaim);
    }

    public async Task UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.GetUserFromUserClaimsAsync(userClaim);
        await _userManager.UpdateUserInformationAsync(user, updateUserDto);
    }

    public async Task UploadImageAsync(ClaimsPrincipal claimsPrincipal, string imageUrl)
    {
        var user = await _userManager.GetUserFromUserClaimsAsync(claimsPrincipal);
        await _userManager.UploadImageAsync(user, imageUrl);
    }
}