using System.Security.Claims;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.UpdateModel;

namespace AnalysisData.Services.Business.Abstraction;

public interface IUserManager
{
    Task<User> GetUserAsync(ClaimsPrincipal userClaim);
    Task<bool> UpdateUserInformationAsync(User user, UpdateUserDto updateUserDto);
    Task<bool> UploadImageAsync(User user, string imageUrl);
}