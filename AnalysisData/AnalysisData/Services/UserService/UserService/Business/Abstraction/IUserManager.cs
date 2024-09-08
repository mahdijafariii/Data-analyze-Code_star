using System.Security.Claims;
using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Models.UserModel;

namespace AnalysisData.Services.UserService.UserService.Business.Abstraction;

public interface IUserManager
{
    Task<User> GetUserFromUserClaimsAsync(ClaimsPrincipal userClaim);
    Task UpdateUserInformationAsync(User user, UpdateUserDto updateUserDto);
    Task<User> GetUserFromEmail(string email);
    Task UploadImageAsync(User user, string imageUrl);
}