namespace AnalysisData.EAV.Service;

public interface IAccessManagementService
{
    Task RevokeUserAccessAsync(List<string> userIds);
    Task GrantUserAccessAsync(List<string> userIds, int fileId);
}