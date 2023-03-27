using Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CleanOnionExample.Middleware;

public class ApiExceptionMiddleware {
  public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger) {
    _next = next;
    _logger = logger;
  }

  private readonly RequestDelegate _next;
  private readonly ILogger<ApiExceptionMiddleware> _logger;

  public async Task InvokeAsync(HttpContext httpContext) {
    try {
      await _next(httpContext);
    } catch (Exception ex) {
      await HandleExceptionAsync(httpContext, ex, _logger);
    }
  }

  private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ApiExceptionMiddleware> logger) {
    int code;
    var actualStatusCode = context.Response.StatusCode;
    var result = exception.Message;
    switch (exception) {
      case ApiException apiException:
        code = StatusCodes.Status400BadRequest;
        result = apiException.Message;
        break;
      case BadRequestException badRequestException:
        code = StatusCodes.Status400BadRequest;
        result = badRequestException.Message;
        break;
      case DeleteFailureException deleteFailureException:
        code = StatusCodes.Status400BadRequest;
        result = deleteFailureException.Message;
        break;
      case KeyNotFoundException keyNotFoundException:
        code = StatusCodes.Status404NotFound;
        result = keyNotFoundException.Message;
        break;
      case NotFoundException notFoundException:
        code = StatusCodes.Status404NotFound;
        result = notFoundException.Message;
        break;
      case ValidationException validationException:
        code = StatusCodes.Status400BadRequest;
        result = JsonSerializer.Serialize(validationException.Errors);
        break;
      default:
        code = StatusCodes.Status500InternalServerError;
        break;
    }

    logger.LogError(result);
    //logger.LogError(exception, result);
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = code;
    return context.Response.WriteAsync(JsonSerializer.Serialize(new {
      StatusCode = code,
      ActualStatusCode = actualStatusCode,
      ErrorMessage = exception.Message
    }));

  }
}

