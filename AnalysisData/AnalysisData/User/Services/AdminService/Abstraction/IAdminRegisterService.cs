using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.AdminService.Abstraction;

public interface IAdminRegisterService
{
    Task RegisterByAdminAsync(UserRegisterDto userRegisterDto);
}