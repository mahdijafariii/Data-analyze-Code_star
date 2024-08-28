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
    private readonly IValidationService _validationService;
    private readonly IRoleRepository _roleRepository;
    private readonly IJwtService _jwtService;

    public AdminService(IUserRepository userRepository, IValidationService validationService,
        IRoleRepository roleRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _validationService = validationService;
        _roleRepository = roleRepository;
        _jwtService = jwtService;
    }

    public async Task UpdateUserInformationByAdminAsync(Guid id, UpdateAdminModel updateAdminModel)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        var checkUsername = await _userRepository.GetUserByUsernameAsync(updateAdminModel.Username);
        var checkEmail = await _userRepository.GetUserByEmailAsync(updateAdminModel.Email);

        if ((checkUsername != null && !user.Equals(checkUsername)) || (checkEmail != null && !user.Equals(checkEmail)))
            throw new DuplicateUserException();

        _validationService.EmailCheck(updateAdminModel.Email);
        _validationService.PhoneNumberCheck(updateAdminModel.PhoneNumber);
        var role = await _roleRepository.GetRoleByNameAsync(updateAdminModel.RoleName);
        if (role == null)
        {
            throw new RoleNotFoundException();
        }

        SetUpdatedInformation(user, updateAdminModel);
        await _jwtService.UpdateUserCookie(user.Username, false);
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

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var isDelete = await _userRepository.DeleteUserAsync(id);
        if (!isDelete)
            throw new UserNotFoundException();
        return true;
    }

    public async Task<int> GetUserCountAsync()
    {
        return await _userRepository.GetUsersCountAsync();
    }

    public async Task<List<UserPaginationModel>> GetAllUserAsync(int page, int limit)
    {
        var users = await _userRepository.GetAllUserPaginationAsync(page, limit);
        var paginationUsers = users.Select(x => new UserPaginationModel()
        {
            Guid = x.Id.ToString(), Username = x.Username, FirstName = x.FirstName, LastName = x.LastName,
            Email = x.Email, PhoneNumber = x.PhoneNumber, RoleName = x.Role.RoleName
        });
        return paginationUsers.ToList();
    }
}