using System.Security.Claims;
using AnalysisData.Exception.UserException;
using AnalysisData.User.JwtService.abstractions;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using AnalysisData.User.Services.EmailService;

namespace AnalysisData.User.Services.UserService;

public class ResetPasswordService
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public ResetPasswordService(IJwtService jwtService, IUserRepository userRepository, IEmailService emailService)
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