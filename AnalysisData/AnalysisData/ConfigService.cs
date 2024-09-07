using System.Globalization;
using AnalysisData.Repositories.GraphRepositories.CategoryRepository;
using AnalysisData.Repositories.GraphRepositories.CategoryRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.FileUploadedRepository;
using AnalysisData.Repositories.GraphRepositories.FileUploadedRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphEdgeRepository;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphEdgeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.UserFileRepository;
using AnalysisData.Repositories.GraphRepositories.UserFileRepository.Abstraction;
using AnalysisData.Repositories.PasswordResetTokensRepository;
using AnalysisData.Repositories.PasswordResetTokensRepository.Abstraction;
using AnalysisData.Repositories.RoleRepository;
using AnalysisData.Repositories.RoleRepository.Abstraction;
using AnalysisData.Repositories.UserRepository;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.CookieService;
using AnalysisData.Services.CookieService.Abstractions;
using AnalysisData.Services.EmailService;
using AnalysisData.Services.EmailService.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;
using AnalysisData.Services.GraphService.Business.EdgeManager;
using AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager;
using AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;
using AnalysisData.Services.GraphService.CategoryService;
using AnalysisData.Services.GraphService.CategoryService.Abstraction;
using AnalysisData.Services.GraphService.FilePermissionService;
using AnalysisData.Services.GraphService.FilePermissionService.Abstractions;
using AnalysisData.Services.GraphService.FilePermissionService.AccessManagement;
using AnalysisData.Services.GraphService.FilePermissionService.AccessManagement.Abstractions;
using AnalysisData.Services.GraphService.FileUploadService;
using AnalysisData.Services.GraphService.FileUploadService.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.AllNodesData;
using AnalysisData.Services.GraphService.GraphServices.AllNodesData.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.NodeAndEdgeInfo;
using AnalysisData.Services.GraphService.GraphServices.NodeAndEdgeInfo.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.Relationship;
using AnalysisData.Services.GraphService.GraphServices.Relationship.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.Search;
using AnalysisData.Services.GraphService.GraphServices.Search.Abstraction;
using AnalysisData.Services.JwtService;
using AnalysisData.Services.JwtService.Abstraction;
using AnalysisData.Services.PermissionService;
using AnalysisData.Services.PermissionService.Abstraction;
using AnalysisData.Services.RoleService;
using AnalysisData.Services.RoleService.Abstraction;
using AnalysisData.Services.S3FileStorageService;
using AnalysisData.Services.S3FileStorageService.Abstraction;
using AnalysisData.Services.TokenService;
using AnalysisData.Services.TokenService.Abstraction;
using AnalysisData.Services.UserService.AdminService;
using AnalysisData.Services.UserService.AdminService.Abstraction;
using AnalysisData.Services.UserService.UserService;
using AnalysisData.Services.UserService.UserService.Abstraction;
using AnalysisData.Services.UserService.UserService.Business;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;
using AnalysisData.Services.ValidationService;
using AnalysisData.Services.ValidationService.Abstraction;
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
        services.AddScoped<ICsvReaderProcessor, CsvReaderProcessorProcessor>();
        services.AddScoped<ICsvReaderManager, CsvReaderManager>();
        services.AddScoped<IHeaderValidatorProcessor, HeaderValidatorProcessorProcessor>();
        services.AddScoped<ICsvHeaderReaderProcessor, CsvHeaderReaderProcessor>();
        services.AddScoped<IEdgeToDbProcessor, EdgeToDbProcessor>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<ILoginManager, LoginManager>();
        services.AddScoped<INodeToDbProcessor, NodeToDbProcessor>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IHeaderProcessor, HeaderProcessor>();
        services.AddScoped<IValidtionPasswordManager, ValidationPasswordManager>();
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
        services.AddScoped<IPasswordHasherManager, PasswordHasherManagerManager>();
        services.AddScoped<IS3FileStorageService, S3FileStorageService>();
        services.AddScoped<IUploadImageService, UploadImageService>();
        services.AddScoped<IValueNodeProcessor, ValueNodeProcessor>();
        services.AddScoped<IEntityEdgeRecordProcessor, EntityEdgeRecordProcessor>();
        services.AddScoped<IValueEdgeProcessor, ValueEdgeProcessor>();
        services.AddScoped<IEdgeToDbProcessor, EdgeToDbProcessor>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IResetPasswordRequestService, ResetPasswordRequestService>();
        services.AddScoped<IValidateTokenService, ValidateTokenService>();

        

        return services;
    }
}