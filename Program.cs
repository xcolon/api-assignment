using UserApi.Services;
using UserApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register custom services
builder.Services.AddScoped<IUserService, UserService>();

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

// Add custom middleware (order matters!)
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ApiKeyAuthenticationMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Add health check endpoint
app.MapHealthChecks("/health");

// Map controllers
app.MapControllers();

app.Run();
