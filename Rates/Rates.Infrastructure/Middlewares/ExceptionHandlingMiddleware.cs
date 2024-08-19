using System.Net;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rates.Infrastructure.Exceptions;

namespace Rates.Infrastructure.MiddleWares;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (IncorrectDataException ex)
        {
            await HandleExceptionAsync(httpContext,
                ex.Message,
                HttpStatusCode.UnprocessableEntity,
                ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            await HandleExceptionAsync(httpContext,
                ex.Message,
                HttpStatusCode.NotFound,
                ex.Message);
        }
        catch (AuthenticationException ex)
        {
            await HandleExceptionAsync(httpContext,
                ex.Message,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext,
                ex.Message,
                HttpStatusCode.InternalServerError,
                "Internal server error");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, string exMsg, HttpStatusCode httpStatusCode,
        string message)
    {
        _logger.LogError(exMsg);

        var response = context.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)httpStatusCode;

        ErrorDto errorDto = new()
        {
            Message = message,
            StatusCode = (int)httpStatusCode
        };

        await response.WriteAsJsonAsync(errorDto);
    }
}