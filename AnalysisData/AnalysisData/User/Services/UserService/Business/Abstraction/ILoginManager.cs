using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.UserService.Abstraction;

public interface ILoginManager
{
    Task<Model.User> LoginAsync(UserLoginDto userLoginDto);
}