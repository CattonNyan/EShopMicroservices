using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Error Message: {ExceptionMessage}, Time of occurrence {Time}",
            exception.Message,
            DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException internalServerException =>
            (
                internalServerException.Details ?? internalServerException.Message,
                internalServerException.GetType().Name,
                StatusCodes.Status500InternalServerError
            ),
            ValidationException fluentValidationException =>
            (
                fluentValidationException.Message,
                fluentValidationException.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            BadRequestException badRequestException =>
            (
                badRequestException.Details ?? badRequestException.Message,
                badRequestException.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            NotFoundException notFoundException =>
            (
                notFoundException.Message,
                notFoundException.GetType().Name,
                StatusCodes.Status404NotFound
            ),
            _ =>
            (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        context.Response.StatusCode = details.StatusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}
