using System.Security.Cryptography;
using System.Text;
using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;

namespace AnalysisData.Services;

public class AdminRegisterService : IAdminRegisterService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidationService _validationService;
    private readonly IRoleRepository _roleRepository;
    private readonly IJwtService _jwtService;

    public AdminRegisterService(IUserRepository userRepository, IValidationService validationService,
        IRoleRepository roleRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _validationService = validationService;
        _roleRepository = roleRepository;
        _jwtService = jwtService;
    }

    public async Task RegisterByAdminAsync(UserRegisterModel userRegisterModel)
    {
        var roleCheck = userRegisterModel.RoleName.ToLower();
        var existingRole = await _roleRepository.GetRoleByNameAsync(roleCheck);
        if (existingRole == null)
        {
            throw new RoleNotFoundException();
        }

        var existingUserByEmail = await _userRepository.GetUserByEmailAsync(userRegisterModel.Email);
        var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(userRegisterModel.Username);
        if (existingUserByEmail != null && existingUserByUsername != null)
            throw new DuplicateUserException();
        _validationService.EmailCheck(userRegisterModel.Email);
        _validationService.PasswordCheck(userRegisterModel.Password);
        if (userRegisterModel.Password != userRegisterModel.ConfirmPassword)
        {
            throw new PasswordMismatchException();
        }

        _validationService.PhoneNumberCheck(userRegisterModel.PhoneNumber);
        if (userRegisterModel.Password != userRegisterModel.ConfirmPassword)
            throw new PasswordMismatchException();


        var user = MakeUser(userRegisterModel, existingRole);
        await _userRepository.AddUserAsync(user);
    }

    private User MakeUser(UserRegisterModel userRegisterModel, Role role)
    {
        var user = new User
        {
            Username = userRegisterModel.Username,
            Password = HashPassword(userRegisterModel.Password),
            FirstName = userRegisterModel.FirstName,
            LastName = userRegisterModel.LastName,
            Email = userRegisterModel.Email,
            PhoneNumber = userRegisterModel.PhoneNumber,
            Role = role,
            ImageURL = null
        };
        return user;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }


    public async Task AddFirstAdminAsync()
    {
        var admin = await _userRepository.GetUserByUsernameAsync("admin");
        if (admin != null)
        {
            throw new AdminExistenceException();
        }

        var adminRole = new Role()
        {
            Id = 1,
            RoleName = "admin".ToLower(),
            RolePolicy = "gold",
        };
        var dataAnalystRole = new Role()
        {
            Id = 2,
            RoleName = "DataAnalyst".ToLower(),
            RolePolicy = "bronze",
        };
        var dataManager = new Role()
        {
            Id = 3,
            RoleName = "DataManager".ToLower(),
            RolePolicy = "silver",
        };

        var firstAdmin = new User()
        {
            Username = "admin", Password = HashPassword("admin"), PhoneNumber = "09131111111",
            FirstName = "admin", LastName = "admin", Email = "admin@gmail.com", Role = adminRole
        };

        await _roleRepository.AddRoleAsync(adminRole);
        await _roleRepository.AddRoleAsync(dataAnalystRole);
        await _roleRepository.AddRoleAsync(dataManager);
        await _userRepository.AddUserAsync(firstAdmin);
    }
}