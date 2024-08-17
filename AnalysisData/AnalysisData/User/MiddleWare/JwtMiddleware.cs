using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AnalysisData.MiddleWare;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string? _jwtSecret;

    public JwtMiddleware(RequestDelegate next,IConfiguration configuration)
    {
        _next = next;
        _jwtSecret = configuration["Jwt:Key"];
    }
    

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/User/login") || context.Request.Path.StartsWithSegments("/api/User/register"))
        {
            await _next(context);
            return;
        }
        var token = context.Request.Cookies["AuthToken"];
        if (token != null)
        {
            try
            {
                AttachUserToContext(context, token);
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token.");
                return;
            }
        }

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
        context.User = principal;
    }
}
