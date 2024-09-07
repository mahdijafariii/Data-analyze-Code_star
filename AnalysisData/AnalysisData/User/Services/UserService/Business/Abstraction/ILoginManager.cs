using AnalysisData.User.Model;
using AnalysisData.User.UserDto.UserDto;


public interface ILoginManager
{
    Task<User> LoginAsync(UserLoginDto userLoginDto);
}