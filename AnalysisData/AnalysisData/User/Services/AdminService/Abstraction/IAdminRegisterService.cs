using AnalysisData.UserDto.UserDto;

namespace AnalysisData.Services.AdminService.Abstraction;

public interface IAdminRegisterService
{
    Task RegisterByAdminAsync(UserRegisterDto userRegisterDto);
}