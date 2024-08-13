using AnalysisData.Data;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.UserManage.LoginModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Services;

public class UserService : ControllerBase 
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<IActionResult> Login([FromBody] UserLoginModel userLoginModel)
    {
        var user = await _userRepository.GetUser(userLoginModel.userName);
        if (user is null)
        {
            return NotFound($"Unable to load user with username '{userLoginModel.userName}'");
        }

        if (user.Password != userLoginModel.password)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(new
        {
            Massage = "welcome",
            Token = "سشی"
        });



    }
    
}