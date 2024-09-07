using System.Security.Claims;

namespace AnalysisData.Services.UserService.UserService.Abstraction;

public interface IResetPasswordRequestService
{
    Task SendRequestToResetPassword(string email);
}