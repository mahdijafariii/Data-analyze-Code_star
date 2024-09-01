using System.Security.Cryptography;
using System.Text;
using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.Services.S3FileStorageService;
using AnalysisData.Services.SecurityPasswordService.Abstraction;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;

namespace AnalysisData.Services;

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
        var existingRole = await _roleRepository.GetRoleByNameAsync(roleCheck);
        if (existingRole == null)
        {
            throw new RoleNotFoundException();
        }

        var existingUserByEmail = await _userRepository.GetUserByEmailAsync(userRegisterDto.Email);
        var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(userRegisterDto.Username);
        if (existingUserByEmail != null && existingUserByUsername != null)
            throw new DuplicateUserException();
        _validationService.EmailCheck(userRegisterDto.Email);
        _validationService.PasswordCheck(userRegisterDto.Password);
        if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
        {
            throw new PasswordMismatchException();
        }

        _validationService.PhoneNumberCheck(userRegisterDto.PhoneNumber);
        if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
            throw new PasswordMismatchException();


        var user = await MakeUser(userRegisterDto, existingRole);
        await _userRepository.AddUserAsync(user);
    }

    private async Task<User> MakeUser(UserRegisterDto userRegisterDto, Role role)
    {
        var user = new User
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