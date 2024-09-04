namespace AnalysisData.Graph.Service.FilePermissionService.AccessMangement;

public interface IAccessManagementService
{
    Task RevokeUserAccessAsync(List<string> userIds);
    Task GrantUserAccessAsync(List<string> userIds, int fileId);
}