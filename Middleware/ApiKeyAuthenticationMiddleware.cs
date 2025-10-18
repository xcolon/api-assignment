namespace UserApi.Middleware
{
    public class ApiKeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyAuthenticationMiddleware> _logger;
        private readonly IConfiguration _configuration;
        private const string API_KEY_HEADER = "X-API-Key";

        public ApiKeyAuthenticationMiddleware(
            RequestDelegate next,
            ILogger<ApiKeyAuthenticationMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for health check and OpenAPI endpoints
            if (context.Request.Path.StartsWithSegments("/health") ||
                context.Request.Path.StartsWithSegments("/openapi") ||
                context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var apiKeyValues))
            {
                _logger.LogWarning("API key missing for {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key is missing");
                return;
            }

            var apiKey = apiKeyValues.FirstOrDefault();
            var validApiKey = _configuration["ApiKey"] ?? "your-secret-api-key-here";

            if (string.IsNullOrEmpty(apiKey) || !apiKey.Equals(validApiKey))
            {
                _logger.LogWarning("Invalid API key provided for {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            await _next(context);
        }
    }
}