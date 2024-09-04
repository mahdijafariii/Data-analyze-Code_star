using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.AdminService.Abstraction;

public interface IAdminService
{
    Task UpdateUserInformationByAdminAsync(Guid id, UpdateAdminDto updateAdminDto);
    Task<bool> DeleteUserAsync(Guid id);
    Task<List<UserPaginationDto>> GetAllUserAsync(int limit, int page);
    Task<int> GetUserCountAsync();
}