using System.Text;
using AnalysisData.Repository.RoleRepository.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AnalysisData;

public class Authorization 
{
    public async Task ConfigureAuthorizationPolicies(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("gold", policy => policy.RequireRole("admin"));
            options.AddPolicy("silver", policy => policy.RequireRole("admin","data-manager"));
            options.AddPolicy("bronze", policy => policy.RequireRole("admin","data-manager","data-analyst"));
        });
    }
}