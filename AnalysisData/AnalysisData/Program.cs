using AnalysisData.CookieService;
using AnalysisData.CookieService.abstractions;
using AnalysisData.Data;
using AnalysisData.EAV.Repository;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.EdgeRepository;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Repository.NodeRepository;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.EAV.Service;
using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.EAV.Service.Business;
using AnalysisData.EAV.Service.Business.Abstraction;
using AnalysisData.JwtService;
using AnalysisData.JwtService.abstractions;
using AnalysisData.MiddleWare;
using AnalysisData.Repository.RoleRepository;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services;
using AnalysisData.Services.Abstraction;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJwtService,JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRegexService, RegexService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IEdgeToDbService, EdgeToDBService>();
builder.Services.AddScoped<INodeToDbService,NodeToDbService>();
builder.Services.AddScoped<ICsvReaderService, CsvReaderService>();
builder.Services.AddScoped<IGraphNodeRepository, GraphNodeRepository>();
builder.Services.AddScoped<IGraphEdgeRepository, GraphEdgeRepository>();
builder.Services.AddScoped<IAttributeNodeRepository, AttributeNodeRepository>();
builder.Services.AddScoped<IValueNodeRepository, ValueNodeRepository>();
builder.Services.AddScoped<IEntityNodeRepository, EntityNodeRepository>();
builder.Services.AddScoped<IEntityEdgeRepository, EntityEdgeRepository>();
builder.Services.AddScoped<IValueEdgeRepository, ValueEdgeRepository>();
builder.Services.AddScoped<IAttributeEdgeRepository, AttributeEdgeRepository>();
builder.Services.AddScoped<IEdgeRecordProcessor, EdgeRecordProcessor>();
builder.Services.AddScoped<IHeaderProcessor, HeaderProcessor>();
builder.Services.AddScoped<INodeRecordProcessor, NodeRecordProcessor>();
builder.Services.AddScoped<IFromToProcessor, FromToProcessor>();
builder.Services.AddScoped<IGraphServiceEav, GraphServiceEav>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUploadFileService, UploadFileService>();
builder.Services.AddScoped<IUploadDataRepository, UploadDataRepository>();
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        corsPolicyBuilder => corsPolicyBuilder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
            
    options.AddPolicy("AllowAngularApp",
        corsPolicyBuilder => corsPolicyBuilder.WithOrigins("https://angular-8926f3d808-analysis-data.apps.ir-thr-ba1.arvancaas.ir")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseCors("AllowAngularApp");
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
