using System.Security.Claims;

namespace AnalysisData.User.Services.UserService.Abstraction;

public interface IResetPasswordService
{
    Task SendRequestToResetPassword(ClaimsPrincipal userClaim);
}