using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AnalysisData;

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
            
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userName = jwtToken.Claims.First(x => x.Type == "Name").Value;
        var roles = jwtToken.Claims.Where(x => x.Type == "Roles").Select(x => x.Value).ToList();
        context.Items["UserName"] = userName;
        context.Items["Roles"] = roles;
    }
}
