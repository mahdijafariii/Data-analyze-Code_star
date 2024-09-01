using System.Security.Claims;
using AnalysisData.Exception;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.Services.Business.Abstraction;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.UpdateModel;

namespace AnalysisData.Services.Business;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;
    private readonly IValidationService _validationService;

    public UserManager(IUserRepository userRepository, IValidationService validationService)
    {
        _userRepository = userRepository;
        _validationService = validationService;
    }

    public async Task<User> GetUserAsync(ClaimsPrincipal userClaim)
    {
        var userName = userClaim.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        return user;
    }

    public async Task<bool> UpdateUserInformationAsync(User user, UpdateUserDto updateUserDto)
    {
        await ValidateEmailAsync(user, updateUserDto.Email);

        _validationService.EmailCheck(updateUserDto.Email);
        _validationService.PhoneNumberCheck(updateUserDto.PhoneNumber);

        await ReplaceUserDetails(user, updateUserDto);

        return true;
    }

    public async Task<bool> UploadImageAsync(User user, string imageUrl)
    {
        user.ImageURL = imageUrl;
        await _userRepository.UpdateUserAsync(user.Id, user);
        return true;
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