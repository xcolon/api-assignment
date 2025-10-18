using System.Diagnostics;

namespace UserApi.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString("N")[..8];

            // Log request start
            _logger.LogInformation(
                "[{RequestId}] Starting {Method} {Path} at {Timestamp}",
                requestId,
                context.Request.Method,
                context.Request.Path,
                DateTime.UtcNow);

            // Add request ID to response headers
            context.Response.Headers["X-Request-ID"] = requestId;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "[{RequestId}] An error occurred processing {Method} {Path}: {ErrorMessage}",
                    requestId,
                    context.Request.Method,
                    context.Request.Path,
                    ex.Message);
                throw;
            }
            finally
            {
                stopwatch.Stop();

                // Log request completion
                _logger.LogInformation(
                    "[{RequestId}] Completed {Method} {Path} with status {StatusCode} in {ElapsedMs}ms",
                    requestId,
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}