using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.Repository.UserRepository.Abstraction;

namespace AnalysisData.EAV.Service;

public class AccessManagementService : IAccessManagementService
{
    private readonly IUserFileRepository _userFileRepository;


    public AccessManagementService(IUserFileRepository userFileRepository)
    {
        _userFileRepository = userFileRepository;
    }

    public async Task GrantUserAccessAsync(List<string> userIds, int fileId)
    {
        foreach (var userId in userIds)
        {
            var userFile = new UserFile() { UserId = Guid.Parse(userId), FileId = fileId };
            await _userFileRepository.AddAsync(userFile);
        }
    }

    public async Task RevokeUserAccessAsync(List<string> userIds)
    {
        foreach (var userId in userIds)
        {
            await _userFileRepository.DeleteByUserIdAsync(userId);
        }
    }
}