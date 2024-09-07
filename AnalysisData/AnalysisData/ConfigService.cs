using System.Globalization;
using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
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
using AnalysisData.Graph.Service.FilePermissionService.AccessManagement;
using AnalysisData.Graph.Service.FileUploadService;
using AnalysisData.Graph.Service.FileUploadService.Abstraction;
using AnalysisData.Graph.Service.GraphServices.AllNodesData;
using AnalysisData.Graph.Service.GraphServices.AllNodesData.Abstraction;
using AnalysisData.Graph.Service.GraphServices.NodeAndEdgeInfo;
using AnalysisData.Graph.Service.GraphServices.NodeAndEdgeInfo.Abstraction;
using AnalysisData.Graph.Service.GraphServices.Relationship;
using AnalysisData.Graph.Service.GraphServices.Relationship.Abstraction;
using AnalysisData.Graph.Service.GraphServices.Search;
using AnalysisData.Graph.Service.GraphServices.Search.Abstraction;
using AnalysisData.User.CookieService;
using AnalysisData.User.CookieService.abstractions;
using AnalysisData.User.JwtService.abstractions;
using AnalysisData.User.Repository.PasswordResetTokensRepository;
using AnalysisData.User.Repository.PasswordResetTokensRepository.Abstraction;
using AnalysisData.User.Repository.RoleRepository;
using AnalysisData.User.Repository.RoleRepository.Abstraction;
using AnalysisData.User.Repository.UserRepository;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using AnalysisData.User.Services.AdminService;
using AnalysisData.User.Services.AdminService.Abstraction;
using AnalysisData.User.Services.EmailService;
using AnalysisData.User.Services.PermissionService;
using AnalysisData.User.Services.PermissionService.Abstraction;
using AnalysisData.User.Services.RoleService;
using AnalysisData.User.Services.RoleService.Abstraction;
using AnalysisData.User.Services.S3FileStorageService;
using AnalysisData.User.Services.S3FileStorageService.Abstraction;
using AnalysisData.User.Services.SecurityPasswordService;
using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.TokenService.Abstraction;
using AnalysisData.User.Services.UserService.Abstraction;
using AnalysisData.User.Services.UserService.Business;
using AnalysisData.User.Services.ValidationService;
using AnalysisData.User.Services.ValidationService.Abstraction;
using CsvHelper;
using CsvHelper.Configuration;

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
        services.AddScoped<IPasswordResetTokensRepository, PasswordResetTokensRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
        services.AddSingleton(csvConfig);
        
        services.AddScoped<CsvReader>(provider =>
        {
            var config = provider.GetRequiredService<CsvConfiguration>();
            var textReader = new StringReader("");
            return new CsvReader(textReader, config);
        });
        services.AddScoped<ICsvReader, CsvReaderWrapper>();
        services.AddScoped<ICsvReaderService, CsvReaderService>();
        services.AddScoped<IHeaderValidator, HeaderValidator>();
        services.AddScoped<ICsvHeaderReader, CsvHeaderReader>();
        services.AddScoped<IEdgeToDbService, EdgeToDbService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<ILoginManager, LoginManager>();
        services.AddScoped<INodeToDbService, NodeToDbService>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddScoped<IHeaderProcessor, HeaderProcessor>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<INodeRecordProcessor, EntityNodeRecordProcessor>();
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
        services.AddScoped<IValueNodeProcessor, ValueNodeProcessor>();
        services.AddScoped<IEntityEdgeRecordProcessor, EntityEdgeRecordProcessor>();
        services.AddScoped<IValueEdgeProcessor, ValueEdgeProcessor>();
        services.AddScoped<IEdgeToDbService, EdgeToDbService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IResetPasswordService, ResetPasswordService>();
        services.AddScoped<IValidateTokenService, ValidateTokenService>();

        

        return services;
    }
}