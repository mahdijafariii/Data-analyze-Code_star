using System.Security.Claims;
using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.UserService.Business.Abstraction;

public interface IUserManager
{
    Task<Model.User> GetUserFromUserClaimsAsync(ClaimsPrincipal userClaim);
    Task UpdateUserInformationAsync(Model.User user, UpdateUserDto updateUserDto);
    Task UploadImageAsync(Model.User user, string imageUrl);
}