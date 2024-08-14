using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;

namespace AnalysisData.Exception;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (NotFoundUserException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status404NotFound);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status400BadRequest);
        }
        catch (TimeoutException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status408RequestTimeout);
        }
        catch (SecurityTokenExpiredException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (SecurityTokenInvalidSignatureException ex)
        {
            await HandleExceptionAsync(httpContext, ex ,StatusCodes.Status401Unauthorized);
        }
        catch (SecurityTokenInvalidAudienceException ex)
        {
            await HandleExceptionAsync(httpContext, ex,StatusCodes.Status401Unauthorized);
        }
    
        
    }

    private Task HandleExceptionAsync(HttpContext context, System.Exception exception, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            statusCode = statusCode,
            message = exception.Message,
        };

        return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}