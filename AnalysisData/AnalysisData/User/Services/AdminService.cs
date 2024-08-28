using System.Security.Cryptography;
using System.Text;
using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.RolePaginationModel;
using AnalysisData.UserManage.UpdateModel;
using AnalysisData.UserManage.UserPaginationModel;

namespace AnalysisData.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IRegexService _regexService;
    private readonly IRoleRepository _roleRepository;
    private readonly IJwtService _jwtService;
    
    public AdminService(IUserRepository userRepository, IRegexService regexService, IRoleRepository roleRepository,IJwtService jwtService)
    {
        _userRepository = userRepository;
        _regexService = regexService;
        _roleRepository = roleRepository;
        _jwtService = jwtService;
    }

    public async Task RegisterByAdmin(UserRegisterModel userRegisterModel)
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
        _regexService.EmailCheck(userRegisterModel.Email);
        _regexService.PasswordCheck(userRegisterModel.Password);
        if (userRegisterModel.Password != userRegisterModel.ConfirmPassword)
        {
            throw new PasswordMismatchException();
        }

        _regexService.PhoneNumberCheck(userRegisterModel.PhoneNumber);
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


    public async Task UpdateUserInformationByAdmin(Guid id, UpdateAdminModel updateAdminModel)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        var checkUsername = await _userRepository.GetUserByUsernameAsync(updateAdminModel.Username);
        var checkEmail = await _userRepository.GetUserByEmailAsync(updateAdminModel.Email);

        if ((checkUsername != null && !user.Equals(checkUsername)) || (checkEmail != null && !user.Equals(checkEmail)))
            throw new DuplicateUserException();

        _regexService.EmailCheck(updateAdminModel.Email);
        _regexService.PhoneNumberCheck(updateAdminModel.PhoneNumber);
        var role = await _roleRepository.GetRoleByNameAsync(updateAdminModel.RoleName);
        if (role == null)
        {
            throw new RoleNotFoundException();
        }

        SetUpdatedInformation(user, updateAdminModel);
        _jwtService.UpdateUserCookie(user.Username, false);
    }

    private void SetUpdatedInformation(User user, UpdateAdminModel updateAdminModel)
    {
        user.FirstName = updateAdminModel.FirstName;
        user.LastName = updateAdminModel.LastName;
        user.Email = updateAdminModel.Email;
        user.PhoneNumber = updateAdminModel.PhoneNumber;
        user.Username = updateAdminModel.Username;
        user.Role.RoleName = updateAdminModel.RoleName;
        _userRepository.UpdateUserAsync(user.Id, user);
        
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        var isDelete = await _userRepository.DeleteUserAsync(id);
        if (!isDelete)
            throw new UserNotFoundException();
        return true;
    }

    public async Task<int> GetUserCount()
    {
        return await _userRepository.GetUsersCountAsync();
    }
    


    public async Task<List<UserPaginationModel>> GetUserPagination(int page, int limit)
    {
        var users = await _userRepository.GetAllUserPaginationAsync(page, limit);
        var paginationUsers = users.Select(x => new UserPaginationModel()
        {
            Guid = x.Id.ToString(), Username = x.Username, FirstName = x.FirstName, LastName = x.LastName,
            Email = x.Email, PhoneNumber = x.PhoneNumber, RoleName = x.Role.RoleName
        });
        return paginationUsers.ToList();
    }
    
    public async Task AddFirstAdmin()
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
            RolePolicy = "boronz",
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