using System.Security.Cryptography;
using System.Text;
using AnalysisData.Exception;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.UpdateModel;
using AnalysisData.UserManage.UserPaginationModel;

namespace AnalysisData.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IRegexService _regexService;

    public AdminService(IUserRepository userRepository, IRegexService regexService)
    {
        _userRepository = userRepository;
        _regexService = regexService;
    }

    public async Task<bool> Register(UserRegisterModel userRegisterModel)
    {
        var existingUserByEmail = _userRepository.GetUserByEmail(userRegisterModel.Email);
        var existingUserByUsername = _userRepository.GetUserByUsername(userRegisterModel.Username);
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

        if (userRegisterModel.RoleName is "admin" or "dataManager" or "systemManager")
        {
            var user = MakeUser(userRegisterModel);
            await _userRepository.AddUser(user);
        }
        else
        {
            throw new RoleNotFoundException();
        }
        
        return true;
    }

    private User MakeUser(UserRegisterModel userRegisterModel)
    {
        var user = new User
        {
            Username = userRegisterModel.Username,
            Password = HashPassword(userRegisterModel.Password),
            FirstName = userRegisterModel.FirstName,
            LastName = userRegisterModel.LastName,
            Email = userRegisterModel.Email,
            PhoneNumber = userRegisterModel.PhoneNumber,
            Role = userRegisterModel.RoleName,
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


    
    public Task<bool> UpdateUserInformationByAdmin(Guid id, UpdateAdminModel updateAdminModel)
    {
        var user = _userRepository.GetUserById(id);
        var checkUsername = _userRepository.GetUserByUsername(updateAdminModel.Username);
        var checkEmail = _userRepository.GetUserByEmail(updateAdminModel.Email);

        if ((checkUsername != null && user.Username!=updateAdminModel.Username) || (checkEmail != null && user.Email!=updateAdminModel.Email))
            throw new DuplicateUserException();

        _regexService.EmailCheck(updateAdminModel.Email);
        _regexService.PhoneNumberCheck(updateAdminModel.PhoneNumber);
        SetUpdatedInformation(user, updateAdminModel);
        return Task.FromResult(true);
    }

    private void SetUpdatedInformation(User user, UpdateAdminModel updateAdminModel)
    {
        user.FirstName = updateAdminModel.FirstName;
        user.LastName = updateAdminModel.LastName;
        user.Email = updateAdminModel.Email;
        user.PhoneNumber = updateAdminModel.PhoneNumber;
        user.Username = updateAdminModel.Username;
        user.Role = updateAdminModel.RoleName;
        _userRepository.UpdateUser(user.Id, user);
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        var isDelete = await _userRepository.DeleteUser(id);
        if (!isDelete)
            throw new UserNotFoundException();
        return true;
    }
    public async Task<int> GetUserCount()
    {
        return await _userRepository.GetUsersCount();
    }
    
    
    public async Task<List<UserPaginationModel>> GetUserPagination(int page , int limit)
    {
        var users = await _userRepository.GetAllUserPagination(page,limit);
        var paginationUsers = users.Select(x => new UserPaginationModel() {Guid = x.Id.ToString(),Username = x.Username , FirstName = x.FirstName , LastName = x.LastName , Email = x.Email , PhoneNumber = x.PhoneNumber , RoleName = x.Role });
        return paginationUsers.ToList();
    }


    public async Task AddFirstAdmin()
    {
        var admin = _userRepository.GetUserByUsername("admin");
        if (admin != null)
        {
            throw new AdminExistenceException();
        }

        var firstAdmin = new User()
        {
            Username = "admin", Password = HashPassword("admin"), PhoneNumber = "09131111111",
            FirstName = "admin", LastName = "admin", Email = "admin@gmail.com", Role = "admin"
        };
        await _userRepository.AddUser(firstAdmin);
    }
}