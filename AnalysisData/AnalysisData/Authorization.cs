using System.Text;
using AnalysisData.Repository.RoleRepository.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AnalysisData;

public class Authorization
{
    private readonly IRoleRepository _roleRepository;

    public Authorization(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task ConfigureServices(IServiceCollection services)
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
                    ValidIssuer = "yourIssuer",
                    ValidAudience = "yourAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecretKey"))
                };
            });

        var goldRoles = await _roleRepository.GetRolesByPolicyAsync("Gold");
        var silverRoles = await _roleRepository.GetRolesByPolicyAsync("Silver");
        var bronzeRoles = await _roleRepository.GetRolesByPolicyAsync("Bronze");
        services.AddAuthorization(options =>
        {
            options.AddPolicy("gold", policy =>
                policy.RequireRole(goldRoles.ToArray()));
            options.AddPolicy("silver", policy =>
                policy.RequireRole(silverRoles.ToArray()));

            options.AddPolicy("bronze", policy =>
                policy.RequireRole(bronzeRoles.ToArray()));
        });
    }
}