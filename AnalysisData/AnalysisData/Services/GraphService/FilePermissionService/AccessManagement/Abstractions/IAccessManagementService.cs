namespace AnalysisData.Services.GraphService.FilePermissionService.AccessManagement.Abstractions;

public interface IAccessManagementService
{
    Task RevokeUserAccessAsync(List<string> userIds);
    Task GrantUserAccessAsync(List<string> userIds, int fileId);
}