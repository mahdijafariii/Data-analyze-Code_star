using System.Security.Claims;
using AnalysisData.User.Model;
using AnalysisData.User.UserDto.UserDto;


public interface IUserManager
{
    Task<User> GetUserFromUserClaimsAsync(ClaimsPrincipal userClaim);
    Task UpdateUserInformationAsync(User user, UpdateUserDto updateUserDto);
    Task UploadImageAsync(User user, string imageUrl);
}