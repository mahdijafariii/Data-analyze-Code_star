using AnalysisData.CookieService.abstractions;
using AnalysisData.Graph.Repository.CategoryRepository;
using AnalysisData.Graph.Repository.CategoryRepository.Abstraction;
using AnalysisData.Graph.Repository.EdgeRepository;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Repository.FileUploadedRepository;
using AnalysisData.Graph.Repository.FileUploadedRepository.Abstraction;
using AnalysisData.Graph.Repository.GraphEdgeRepository;
using AnalysisData.Graph.Repository.GraphNodeRepository;
using AnalysisData.Graph.Repository.NodeRepository;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Repository.UserFileRepository;
using AnalysisData.Graph.Repository.UserFileRepository.Abstraction;
using AnalysisData.Graph.Service.CategoryService;
using AnalysisData.Graph.Service.CategoryService.Abstraction;
using AnalysisData.Graph.Service.FilePermissionService;
using AnalysisData.Graph.Service.FilePermissionService.AccessMangement;
using AnalysisData.Graph.Service.FileUploadService;
using AnalysisData.Graph.Service.FileUploadService.Abstraction;
using AnalysisData.Graph.Service.GraphServices.AllNodesData;
using AnalysisData.Graph.Service.GraphServices.NodeAndEdgeInfo;
using AnalysisData.Graph.Service.GraphServices.Relationship;
using AnalysisData.Graph.Service.GraphServices.Search;
using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.RoleRepository;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services;
using AnalysisData.Services.AdminService;
using AnalysisData.Services.AdminService.Abstraction;
using AnalysisData.Services.PemissionService;
using AnalysisData.Services.PemissionService.Abstraction;
using AnalysisData.Services.RoleService;
using AnalysisData.Services.RoleService.Abstraction;
using AnalysisData.Services.S3FileStorageService;
using AnalysisData.Services.S3FileStorageService.Abstraction;
using AnalysisData.Services.SecurityPasswordService;
using AnalysisData.Services.SecurityPasswordService.Abstraction;
using AnalysisData.Services.UserService;
using AnalysisData.Services.UserService.Abstraction;
using AnalysisData.Services.ValidationService;
using AnalysisData.Services.ValidationService.Abstraction;
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
        services.AddScoped<IJwtService, JwtService.JwtService>();
        services.AddScoped<ICookieService, CookieService.CookieService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IValidationService, ValidationService>();
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
        services.AddScoped<IS3FileStorageService, S3FileStorageService>();
        services.AddScoped<IUploadImageService, UploadImageService>();


        return services;
    }
}