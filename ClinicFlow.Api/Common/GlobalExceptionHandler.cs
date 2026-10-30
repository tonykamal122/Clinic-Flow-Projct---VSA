using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.Api.Common ;

    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "unhandled exception .TraceId: {TraceId}", httpContext.TraceIdentifier);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred."
            };
            
            var response = httpContext.Response;
            response.StatusCode = problemDetails.Status.Value;

            await response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);
            
            return true;
        }
    }