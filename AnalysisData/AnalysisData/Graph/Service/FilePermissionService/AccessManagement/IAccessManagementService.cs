namespace AnalysisData.Graph.Service.FilePermissionService.AccessManagement;

public interface IAccessManagementService
{
    Task RevokeUserAccessAsync(List<string> userIds);
    Task GrantUserAccessAsync(List<string> userIds, int fileId);
}