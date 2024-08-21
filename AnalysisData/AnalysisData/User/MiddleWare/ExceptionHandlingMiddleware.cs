using System.ComponentModel.DataAnnotations;
using AnalysisData.Exception;
using Microsoft.IdentityModel.Tokens;

namespace AnalysisData.MiddleWare;

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
        catch (AggregateException aggEx)
        {
            foreach (var ex in aggEx.InnerExceptions)
            {
                if (ex is UserNotFoundException)
                {
                    await HandleExceptionAsync(httpContext, ex, StatusCodes.Status404NotFound);
                    return;
                }
                if (ex is RoleNotFoundException)
                {
                    await HandleExceptionAsync(httpContext, ex, StatusCodes.Status404NotFound);
                    return;
                }
                if (ex is PasswordMismatchException)
                {
                    await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
                }
            }
        }
        catch (UserNotFoundException ex)
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
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (SecurityTokenInvalidAudienceException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (InvalidPasswordException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (DuplicateUserException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (PasswordMismatchException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (InvalidEmailFormatException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (InvalidPasswordFormatException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (InvalidPhoneNumberFormatException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (RoleNotFoundException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized);
        }
        catch (AdminExistenceException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status403Forbidden);
        }
        catch (CategoryResultNotFoundException ex)
        {
            await HandleExceptionAsync(httpContext, ex, StatusCodes.Status404NotFound);
        }
    }
    
    // private static List<Type> GetCustomExceptions()
    // {
    //     var baseType = typeof(System.Exception);
    //     var exceptionTypes = AppDomain.CurrentDomain.GetAssemblies()
    //         .SelectMany(assembly => assembly.GetTypes())
    //         .Where(type => type.IsSubclassOf(baseType) && !type.IsAbstract)
    //         .ToList();
    //
    //     return exceptionTypes;
    // }


    private Task HandleExceptionAsync(HttpContext context, System.Exception exception, int _statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = _statusCode;

        var response = new
        {
            statusCode = _statusCode,
            message = exception.Message,
        };

        return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}