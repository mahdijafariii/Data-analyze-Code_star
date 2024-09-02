using System.Text;
using AnalysisData;
using AnalysisData.Data;
using AnalysisData.MiddleWare;
using AnalysisData.Repository.RoleRepository.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.json").AddEnvironmentVariables();
var connectionString = builder.Configuration["CONNECTION_STRING"];

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
// builder.Services.AddScoped<Authorization>();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();




var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var authorization = services.GetRequiredService<Authorization>();
//     await authorization.ConfigureAuthorizationPolicies(builder.Services); 
// }


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
// app.UseCors("AllowAngularApp");
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(x => x.AllowCredentials().AllowAnyHeader().AllowAnyMethod()
    .SetIsOriginAllowed(x => true));
app.Run();