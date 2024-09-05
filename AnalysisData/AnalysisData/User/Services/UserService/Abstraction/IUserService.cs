using System.Security.Claims;
using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.UserService.Abstraction;

public interface IUserService
{
    Task<Model.User> LoginAsync(UserLoginDto userLoginDto);
    Task<Model.User> GetUserAsync(ClaimsPrincipal userClaim);
    Task ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword);
    Task UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserDto updateUserDto);
    Task NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword);
}