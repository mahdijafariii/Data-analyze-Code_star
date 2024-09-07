using System.Security.Claims;
using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Exception.UserException;
using AnalysisData.Models.UserModel;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;
using AnalysisData.Services.ValidationService.Abstraction;

namespace AnalysisData.Services.UserService.UserService.Business;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;
    private readonly IValidationService _validationService;

    public UserManager(IUserRepository userRepository, IValidationService validationService)
    {
        _userRepository = userRepository;
        _validationService = validationService;
    }

    public async Task<User> GetUserFromUserClaimsAsync(ClaimsPrincipal userClaim)
    {
        var userName = userClaim.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        return user;
    }

    public async Task UpdateUserInformationAsync(User user, UpdateUserDto updateUserDto)
    {
        await ValidateEmailAsync(user, updateUserDto.Email);

        _validationService.EmailCheck(updateUserDto.Email);
        _validationService.PhoneNumberCheck(updateUserDto.PhoneNumber);

        await ReplaceUserDetails(user, updateUserDto);
    }

    public async Task UploadImageAsync(User user, string imageUrl)
    {
        user.ImageURL = imageUrl;
        await _userRepository.UpdateUserAsync(user.Id, user);
    }

    private async Task ValidateEmailAsync(User user, string newEmail)
    {
        var checkEmail = await _userRepository.GetUserByEmailAsync(newEmail);
        if (checkEmail != null && user.Email != newEmail)
        {
            throw new DuplicateUserException();
        }
    }

    private async Task ReplaceUserDetails(User user, UpdateUserDto updateUserDto)
    {
        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.Email = updateUserDto.Email;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        await _userRepository.UpdateUserAsync(user.Id, user);
    }
}