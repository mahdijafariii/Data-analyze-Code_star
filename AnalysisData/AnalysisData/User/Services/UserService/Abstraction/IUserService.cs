using System.Security.Claims;
using AnalysisData.Model;
using AnalysisData.UserDto.UserDto;

namespace AnalysisData.Services.UserService.Abstraction;

public interface IUserService
{
    Task<User> LoginAsync(UserLoginDto userLoginDto);
    Task<User> GetUserAsync(ClaimsPrincipal userClaim);
    Task<bool> ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword);
    Task<bool> UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserDto updateUserDto);
    Task<bool> NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword);
}