using AnalysisData.CookieService.abstractions;
using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Repository.UserRoleRepository.Abstraction;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;

namespace AnalysisData.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ICookieService _cookieService;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, ICookieService cookieService,IJwtService jwtService,IUserRoleRepository userRoleRepository,IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _cookieService = cookieService;
        _jwtService = jwtService;
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
    }
    public async Task<List<string>> Login(UserLoginModel userLoginModel)
    {
        var user = await _userRepository.GetUser(userLoginModel.userName);
        if (user == null )
        {
            throw new NotFoundUserException(); 
        }
        if (user.Password != userLoginModel.password)
        {
            throw new InvalidPasswordException();
        }
        var token = await _jwtService.GenerateJwtToken(userLoginModel.userName);

        _cookieService.SetCookie("AuthToken", token, userLoginModel.rememberMe);

        var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();

        return roles;
    }
    public async Task<bool> Register(UserRegisterModel userRegisterModel)
    {
        var allUsers = await _userRepository.GetAllUser();
        var existingUser = allUsers.FirstOrDefault(u =>
            u.Username == userRegisterModel.Username || u.Email == userRegisterModel.Email);
        if (existingUser != null)
            return false; //exception duplicate user(email or username)
        if (userRegisterModel.Password != userRegisterModel.ConfirmPassword)
            return false; //exception not equal

        var user = new User
        {
            Username = userRegisterModel.Username,
            Password = userRegisterModel.Password, //hash password needed
            FirstName = userRegisterModel.FirstName,
            LastName = userRegisterModel.LastName,
            Email = userRegisterModel.Email,
            PhoneNumber = userRegisterModel.PhoneNumber,
            ImageURL = null
        };
        _userRepository.AddUser(user);
        var role = await _roleRepository.GetRoleByName(userRegisterModel.RoleName);
        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        };
        await _userRoleRepository.Add(userRole);
        return true;
    }
    
}