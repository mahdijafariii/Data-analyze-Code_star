using AnalysisData.CookieService.abstractions;
using AnalysisData.EAV.Repository;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.CategoryRepository;
using AnalysisData.EAV.Repository.CategoryRepository.asbtraction;
using AnalysisData.EAV.Repository.EdgeRepository;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Repository.FileUploadedRepository;
using AnalysisData.EAV.Repository.NodeRepository;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.EAV.Service;
using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.EAV.Service.Business;
using AnalysisData.EAV.Service.Business.Abstraction;
using AnalysisData.EAV.Service.GraphServices.NodeAndEdgeServices;
using AnalysisData.EAV.Service.GraphServices.Relationship;
using AnalysisData.EAV.Service.GraphSevices;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.RoleRepository;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services;
using AnalysisData.Services.Abstraction;
using AnalysisData.Services.Business;
using AnalysisData.Services.Business.Abstraction;
using AnalysisData.Services.SecurityPasswordService;
using AnalysisData.Services.SecurityPasswordService.Abstraction;
using Microsoft.AspNetCore.Identity;

namespace AnalysisData;

public static class ConfigService
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGraphNodeRepository, GraphNodeRepository>();
        services.AddScoped<IAttributeNodeRepository, AttributeNodeRepository>();
        services.AddScoped<IGraphEdgeRepository, GraphEdgeRepository>();
        services.AddScoped<IValueNodeRepository, ValueNodeRepository>();
        services.AddScoped<IAttributeEdgeRepository, AttributeEdgeRepository>();
        services.AddScoped<IEntityEdgeRepository, EntityEdgeRepository>();
        services.AddScoped<IEntityNodeRepository, EntityNodeRepository>();
        services.AddScoped<IValueEdgeRepository, ValueEdgeRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IFileUploadedRepository, FileUploadedRepository>();
        services.AddScoped<IUserFileRepository, UserFileRepository>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddScoped<IPasswordService, PasswordService>();  
        services.AddScoped<IJwtService, JwtService.JwtService>();
        services.AddScoped<ICookieService, CookieService.CookieService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<ILoginManager, LoginManager>();
        services.AddScoped<IEdgeToDbService, EdgeToDbService>();
        services.AddScoped<INodeToDbService, NodeToDbService>();
        services.AddScoped<ICsvReaderService, CsvReaderService>();
        services.AddScoped<IEdgeRecordProcessor, EdgeRecordProcessor>();
        services.AddScoped<IHeaderProcessor, HeaderProcessor>();
        services.AddScoped<INodeRecordProcessor, NodeRecordProcessor>();
        services.AddScoped<IFromToProcessor, FromToProcessor>();
        services.AddScoped<INodePaginationService, NodePaginationService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<INodeAndEdgeInfo, NodeAndEdgeInfo>();
        services.AddScoped<IGraphRelationService, GraphRelationService>();
        services.AddScoped<IGraphSearchService, GraphSearchService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IUploadFileService, UploadFileService>();
        services.AddScoped<IFilePermissionService, FilePermissionService>();
        services.AddScoped<IRoleManagementService, RoleManagementService>();
        services.AddScoped<IAccessManagementService, AccessManagementService>();
        services.AddScoped<IAdminRegisterService, AdminRegisterService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }
}