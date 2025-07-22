using SubscriptionAnalytics.Shared.DTOs;
using SubscriptionAnalytics.Shared.Exceptions;
using System.Net;
using System.Text.Json;

namespace SubscriptionAnalytics.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);

        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = CreateErrorResponse(context, exception);
        var statusCode = GetStatusCode(exception);

        response.StatusCode = statusCode;

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        });

        await response.WriteAsync(jsonResponse);
    }

    private ErrorResponse CreateErrorResponse(HttpContext context, Exception exception)
    {
        var errorResponse = new ErrorResponse
        {
            TraceId = context.TraceIdentifier,
            Timestamp = DateTime.UtcNow
        };

        switch (exception)
        {
            case ValidationException validationEx:
                var validationResponse = new ValidationErrorResponse
                {
                    TraceId = context.TraceIdentifier,
                    Timestamp = DateTime.UtcNow,
                    Message = validationEx.Message,
                    Type = validationEx.ErrorCode,
                    ValidationErrors = validationEx.ValidationErrors
                };
                return validationResponse;

            case NotFoundException notFoundEx:
                errorResponse.Message = notFoundEx.Message;
                errorResponse.Type = notFoundEx.ErrorCode;
                errorResponse.Extensions = new Dictionary<string, object>
                {
                    ["errorCode"] = notFoundEx.ErrorCode
                };
                break;

            case UnauthorizedException unauthorizedEx:
                errorResponse.Message = unauthorizedEx.Message;
                errorResponse.Type = unauthorizedEx.ErrorCode;
                errorResponse.Extensions = new Dictionary<string, object>
                {
                    ["errorCode"] = unauthorizedEx.ErrorCode
                };
                break;

            case ForbiddenException forbiddenEx:
                errorResponse.Message = forbiddenEx.Message;
                errorResponse.Type = forbiddenEx.ErrorCode;
                errorResponse.Extensions = new Dictionary<string, object>
                {
                    ["errorCode"] = forbiddenEx.ErrorCode
                };
                break;

            case ArgumentException argEx:
                errorResponse.Message = argEx.Message;
                errorResponse.Type = "INVALID_ARGUMENT";
                break;

            case InvalidOperationException invalidOpEx:
                errorResponse.Message = invalidOpEx.Message;
                errorResponse.Type = "INVALID_OPERATION";
                break;

            default:
                errorResponse.Message = _environment.IsDevelopment() 
                    ? exception.Message 
                    : "An unexpected error occurred";
                errorResponse.Type = exception is BusinessException bex ? bex.ErrorCode : "INTERNAL_SERVER_ERROR";
                
                if (_environment.IsDevelopment())
                {
                    errorResponse.Details = exception.StackTrace;
                    errorResponse.Extensions = new Dictionary<string, object>
                    {
                        ["exceptionType"] = exception.GetType().Name
                    };
                }
                break;
        }

        return errorResponse;
    }

    private int GetStatusCode(Exception exception)
    {
        // TODO: Enhance this logic for more granular exception mapping, including generic BusinessException
        if (exception is ValidationException)
            return (int)HttpStatusCode.BadRequest;
        if (exception is NotFoundException)
            return (int)HttpStatusCode.NotFound;
        if (exception is UnauthorizedException)
            return (int)HttpStatusCode.Unauthorized;
        if (exception is ForbiddenException)
            return (int)HttpStatusCode.Forbidden;
        if (exception is BusinessException)
            return (int)HttpStatusCode.BadRequest;
        if (exception is ArgumentException)
            return (int)HttpStatusCode.BadRequest;
        if (exception is InvalidOperationException)
            return (int)HttpStatusCode.BadRequest;
        return (int)HttpStatusCode.InternalServerError;
    }
} 