using AnalysisData.Exception.UserException;
using AnalysisData.User.Model;
using AnalysisData.User.Repository.RoleRepository.Abstraction;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using AnalysisData.User.Services.AdminService.Abstraction;
using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.ValidationService.Abstraction;
using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.AdminService;

public class AdminRegisterService : IAdminRegisterService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidationService _validationService;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;


    public AdminRegisterService(IUserRepository userRepository, IValidationService validationService,
        IRoleRepository roleRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _validationService = validationService;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
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
        var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(userRegisterDto.Username);
        if (existingUserByEmail != null || existingUserByUsername != null)
            throw new DuplicateUserException();
    }

    private async Task<Model.User> MakeUser(UserRegisterDto userRegisterDto, Role role)
    {
        var user = new Model.User
        {
            Username = userRegisterDto.Username,
            Password = _passwordHasher.HashPassword(userRegisterDto.Password),
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