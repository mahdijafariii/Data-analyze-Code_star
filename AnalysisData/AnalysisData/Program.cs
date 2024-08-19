using System.Text;
using AnalysisData.CookieService;
using AnalysisData.CookieService.abstractions;
using AnalysisData.Data;
using AnalysisData.EAV.Repository;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.FileManage.Service;
using AnalysisData.FileManage.Service.Business;
using AnalysisData.Graph;
using AnalysisData.Graph.DataProcessService;
using AnalysisData.Graph.Services;
using AnalysisData.JwtService;
using AnalysisData.JwtService.abstractions;
using AnalysisData.MiddleWare;
using AnalysisData.Repository.AccountRepository;
using AnalysisData.Repository.AccountRepository.Abstraction;
using AnalysisData.Repository.TransactionRepository;
using AnalysisData.Repository.TransactionRepository.Abstraction;
using AnalysisData.Repository.UserRepository;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services;
using AnalysisData.Services.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Services registration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Authentication and Authorization
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRegexService, RegexService>();
builder.Services.AddScoped<IDataProcessor, DataReadProcessor>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IGraphUtility, GraphUtility>();
builder.Services.AddScoped<IGraphService, GraphServices>();
builder.Services.AddScoped<INodeService, NodeService>();
builder.Services.AddScoped<IEntityNodeRepository, EntityNodeRepository>();
builder.Services.AddScoped<IAttributeNodeRepository, AttributeNodeRepository>();
builder.Services.AddScoped<IValueNodeRepository, ValueNodeRepository>();
builder.Services.AddScoped<IEntityEdgeRepository, EntityEdgeRepository>();
builder.Services.AddScoped<IAttributeEdgeRepository, AttributeEdgeRepository>();
builder.Services.AddScoped<IValueEdgeRepository, ValueEdgeRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICsvReaderService, CsvReaderService>();
builder.Services.AddScoped<IHeaderProcessor, HeaderProcessor>();
builder.Services.AddScoped<IRecordProcessor, RecordProcessor>();

ConfigureServices(builder.Services, builder.Configuration);

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();


    services.AddScoped<IJwtService, JwtService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<ICookieService, CookieService>();


    services.AddScoped<IDataProcessor, DataReadProcessor>();
    services.AddScoped<ICsvReaderService, CsvReaderService>();
    services.AddScoped<IHeaderProcessor, HeaderProcessor>();
    services.AddScoped<IRecordProcessor, RecordProcessor>();


    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IAccountRepository, AccountRepository>();
    services.AddScoped<ITransactionRepository, TransactionRepository>();
    services.AddScoped<IEntityNodeRepository, EntityNodeRepository>();
    services.AddScoped<IAttributeNodeRepository, AttributeNodeRepository>();
    services.AddScoped<IValueNodeRepository, ValueNodeRepository>();
    services.AddScoped<IEntityEdgeRepository, EntityEdgeRepository>();
    services.AddScoped<IAttributeEdgeRepository, AttributeEdgeRepository>();
    services.AddScoped<IValueEdgeRepository, ValueEdgeRepository>();


    services.AddScoped<IPermissionService, PermissionService>();
    services.AddScoped<IRegexService, RegexService>();
    services.AddScoped<IGraphUtility, GraphUtility>();
    services.AddScoped<IGraphService, GraphServices>();
    services.AddScoped<INodeService, NodeService>();
    services.AddScoped<IAdminService, AdminService>();


    services.AddHttpContextAccessor();


    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")),
        ServiceLifetime.Scoped);
}


builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        corsPolicyBuilder => corsPolicyBuilder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var cookie = context.Request.Cookies["AuthToken"];
                if (!string.IsNullOrEmpty(cookie))
                {
                    context.Token = cookie;
                }
        
                return Task.CompletedTask;
            }
        };
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
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();