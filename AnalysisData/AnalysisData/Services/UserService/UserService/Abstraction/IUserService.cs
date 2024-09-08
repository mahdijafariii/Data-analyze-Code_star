using System.Security.Claims;
using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Models.UserModel;

namespace AnalysisData.Services.UserService.UserService.Abstraction;

public interface IUserService
{
    Task<User> LoginAsync(UserLoginDto userLoginDto);
    Task<User> GetUserAsync(ClaimsPrincipal userClaim);
    Task ResetPasswordAsync(string email, string password, string confirmPassword , string resetPasswordToken);
    Task UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserDto updateUserDto);
    Task NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword);
}