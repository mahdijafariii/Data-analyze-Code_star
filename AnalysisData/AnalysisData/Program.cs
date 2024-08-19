
using System.Reflection;
using System.Text;
using AnalysisData;
using AnalysisData.CookieService;
using AnalysisData.CookieService.abstractions;
using AnalysisData.Data;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.Exception;
using AnalysisData.FileManage.Service;
using AnalysisData.FileManage.Service.Abstraction;
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
builder.Services.AddScoped<IDataProcessor, DataReadProcessor>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IGraphUtility, GraphUtility>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IGraphService, GraphServices>();
////
builder.Services.AddScoped<IUploadRepository, UploadRepository>();
builder.Services.AddScoped<IFileManagementService, FileManagementService>();
builder.Services.AddScoped<IEntityNodeRepository, EntityNodeRepository>();
builder.Services.AddScoped<IAttributeNodeRepository, AttributeNodeRepository>();
builder.Services.AddScoped<IValueNodeRepository, ValueNodeRepository>();
builder.Services.AddScoped<IEntityEdgeRepository, EntityEdgeRepository>();
builder.Services.AddScoped<IAttributeEdgeRepository, AttributeEdgeRepository>();
builder.Services.AddScoped<IValueEdgeRepository, ValueEdgeRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
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