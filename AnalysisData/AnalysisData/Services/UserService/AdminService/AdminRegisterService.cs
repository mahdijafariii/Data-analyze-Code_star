using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Exception.PasswordException;
using AnalysisData.Exception.RoleException;
using AnalysisData.Exception.UserException;
using AnalysisData.Models.UserModel;
using AnalysisData.Repositories.RoleRepository.Abstraction;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.UserService.AdminService.Abstraction;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;
using AnalysisData.Services.ValidationService.Abstraction;

namespace AnalysisData.Services.UserService.AdminService;

public class AdminRegisterService : IAdminRegisterService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidationService _validationService;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasherManager _passwordHasherManager;


    public AdminRegisterService(IUserRepository userRepository, IValidationService validationService,
        IRoleRepository roleRepository, IPasswordHasherManager passwordHasherManager)
    {
        _userRepository = userRepository;
        _validationService = validationService;
        _roleRepository = roleRepository;
        _passwordHasherManager = passwordHasherManager;
    }

    public async Task RegisterByAdminAsync(UserRegisterDto userRegisterDto)
    {
        var roleCheck = userRegisterDto.RoleName.ToLower();
        var existingRole = await CheckExistenceRoleAsync(roleCheck);
        await ValidateUserRegistrationDataAsync(userRegisterDto);
        var user = await MakeUser(userRegisterDto, existingRole);
        await _userRepository.AddUserAsync(user);
    }

    private async Task ValidateUserRegistrationDataAsync(UserRegisterDto userRegisterDto)
    {
        await CheckForDuplicateUserAsync(userRegisterDto);
        ValidateInputFormat(userRegisterDto);
        if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
            throw new PasswordMismatchException();
    }

    private void ValidateInputFormat(UserRegisterDto userRegisterDto)
    {
        _validationService.EmailCheck(userRegisterDto.Email);
        _validationService.PasswordCheck(userRegisterDto.Password);
        _validationService.PhoneNumberCheck(userRegisterDto.PhoneNumber);
    }

    private async Task<Role> CheckExistenceRoleAsync(string roleCheck)
    {
        var existingRole = await _roleRepository.GetRoleByNameAsync(roleCheck);
        if (existingRole == null)
        {
            throw new RoleNotFoundException();
        }

        return existingRole;
    }

    private async Task CheckForDuplicateUserAsync(UserRegisterDto userRegisterDto)
    {
        var existingUserByEmail = await _userRepository.GetUserByEmailAsync(userRegisterDto.Email);
        var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(userRegisterDto.UserName);
        var existingUserByPhoneNumber = await _userRepository.GetUserByPhoneNumberAsync(userRegisterDto.PhoneNumber);
        if (existingUserByEmail != null || existingUserByUsername != null || existingUserByPhoneNumber!=null)
            throw new DuplicateUserException();
    }

    private async Task<User> MakeUser(UserRegisterDto userRegisterDto, Role role)
    {
        var user = new User
        {
            Username = userRegisterDto.UserName,
            Password = _passwordHasherManager.HashPassword(userRegisterDto.Password),
            FirstName = userRegisterDto.FirstName,
            LastName = userRegisterDto.LastName,
            Email = userRegisterDto.Email,
            PhoneNumber = userRegisterDto.PhoneNumber,
            Role = role,
            ImageURL = null
        };
        return user;
    }
}