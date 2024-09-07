using AnalysisData.Dtos.UserDto.UserDto;

namespace AnalysisData.Services.UserService.AdminService.Abstraction;

public interface IAdminRegisterService
{
    Task RegisterByAdminAsync(UserRegisterDto userRegisterDto);
}