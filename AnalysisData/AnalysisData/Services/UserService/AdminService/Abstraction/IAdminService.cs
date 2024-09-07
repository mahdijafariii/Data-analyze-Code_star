using AnalysisData.Dtos.UserDto.UserDto;

namespace AnalysisData.Services.UserService.AdminService.Abstraction;

public interface IAdminService
{
    Task UpdateUserInformationByAdminAsync(Guid id, UpdateAdminDto updateAdminDto);
    Task<bool> DeleteUserAsync(Guid id);
    Task<List<UserPaginationDto>> GetAllUserAsync(int limit, int page);
    Task<int> GetUserCountAsync();
}