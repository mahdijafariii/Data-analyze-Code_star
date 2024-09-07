using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Models.UserModel;

namespace AnalysisData.Services.UserService.UserService.Business.Abstraction;

public interface ILoginManager
{
    Task<User> LoginAsync(UserLoginDto userLoginDto);
}