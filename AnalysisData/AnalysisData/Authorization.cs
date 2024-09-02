using System.Text;
using AnalysisData.Repository.RoleRepository.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace AnalysisData;

public class Authorization
{
    private readonly IRoleRepository _roleRepository;

    public Authorization(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "",
                    ValidAudience = "",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"))
                };
            });

        services.AddAuthorization(); 
    }

    public async Task InitializeRolesAsync(IServiceProvider serviceProvider)
    {
        var goldRoles = await _roleRepository.GetRolesByPolicyAsync("Gold");
        var silverRoles = await _roleRepository.GetRolesByPolicyAsync("Silver");
        var bronzeRoles = await _roleRepository.GetRolesByPolicyAsync("Bronze");

        var authorizationOptions = serviceProvider.GetRequiredService<AuthorizationOptions>();

        authorizationOptions.AddPolicy("gold", policy => policy.RequireRole(goldRoles.ToArray()));
        authorizationOptions.AddPolicy("silver", policy => policy.RequireRole(silverRoles.ToArray()));
        authorizationOptions.AddPolicy("bronze", policy => policy.RequireRole(bronzeRoles.ToArray()));
    }
}