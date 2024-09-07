using System.Security.Claims;
using AnalysisData.User.Model;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.TokenService.Abstraction;
using AnalysisData.User.Services.UserService.Abstraction;
using AnalysisData.User.Services.ValidationService.Abstraction;
using AnalysisData.User.UserDto.UserDto;


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
    public async Task ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword,string resetPasswordToken)
    {
        var user = await _userManager.GetUserFromUserClaimsAsync(userClaim);
        await _passwordManager.ResetPasswordAsync(user, password, confirmPassword,resetPasswordToken);
    }

    public async Task NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword)
    {
        var user = await _userManager.GetUserFromUserClaimsAsync(userClaim);
        await _passwordManager.NewPasswordAsync(user, oldPassword, password, confirmPassword);
    }

    public async Task<User> LoginAsync(UserLoginDto userLoginDto)
    {
        return await _loginManager.LoginAsync(userLoginDto);
    }

    public async Task<User> GetUserAsync(ClaimsPrincipal userClaim)
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