using System.Security.Claims;
using AnalysisData.Exception.UserException;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.EmailService.Abstraction;
using AnalysisData.Services.JwtService.Abstraction;
using AnalysisData.Services.UserService.UserService.Abstraction;
using AnalysisData.Services.ValidationService.Abstraction;

namespace AnalysisData.Services.UserService.UserService;

public class ResetPasswordRequestService : IResetPasswordRequestService
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IValidationService _validationService;

    public ResetPasswordRequestService(IJwtService jwtService, IUserRepository userRepository,
        IEmailService emailService, IValidationService validationService)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _emailService = emailService;
        _validationService = validationService;
    }


    public async Task SendRequestToResetPassword(string email)
    {
        _validationService.EmailCheck(email);
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var token = await _jwtService.RequestResetPassword(user);
        await _emailService.SendPasswordResetEmail(user.Email, "https://myfronti.abriment.com/reset-password",token);
    }
}