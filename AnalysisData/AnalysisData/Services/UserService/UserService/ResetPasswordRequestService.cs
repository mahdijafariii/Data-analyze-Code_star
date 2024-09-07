using System.Security.Claims;
using AnalysisData.Exception.UserException;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.EmailService.Abstraction;
using AnalysisData.Services.JwtService.Abstraction;
using AnalysisData.Services.UserService.UserService.Abstraction;

namespace AnalysisData.Services.UserService.UserService;

public class ResetPasswordRequestService : IResetPasswordRequestService
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public ResetPasswordRequestService(IJwtService jwtService, IUserRepository userRepository, IEmailService emailService)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _emailService = emailService;
    }


    public async Task SendRequestToResetPassword(ClaimsPrincipal userClaim)
    {
        var userName = userClaim.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        if (user is null)
        {
            throw new UserNotFoundException();
        }
        await _jwtService.RequestResetPassword(user);
        await _emailService.SendPasswordResetEmail(user.Email, "www.digikala.com");
    }
}