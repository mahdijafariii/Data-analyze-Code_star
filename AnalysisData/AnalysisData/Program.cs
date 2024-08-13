using AnalysisData;
using AnalysisData.CookieSevice;
using AnalysisData.CookieSevice.abstractions;
using AnalysisData.Data;
using AnalysisData.JwtService;
using AnalysisData.Repository.RoleRepository;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJwtService,JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddHttpContextAccessor();





builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();