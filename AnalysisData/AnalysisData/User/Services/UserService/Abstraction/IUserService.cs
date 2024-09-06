using System.Security.Claims;
using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.UserService.Abstraction;

public interface IUserService
{
    Task<Model.User> LoginAsync(UserLoginDto userLoginDto);
    Task<Model.User> GetUserAsync(ClaimsPrincipal userClaim);
    Task<bool> ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword , string resetPasswordToken);
    Task<bool> UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserDto updateUserDto);
    Task<bool> NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword);
}