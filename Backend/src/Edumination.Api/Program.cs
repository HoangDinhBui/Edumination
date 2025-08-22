using Education.Repositories;
using Education.Repositories.Interfaces;
using Education.Services;
using Edumination.Api.Common.Services;
using Edumination.Api.Features.Auth;
using Edumination.Api.Features.Auth.Requests;
using Edumination.Api.Features.Auth.Services;
using Edumination.Api.Infrastructure.External.Email;
using Edumination.Api.Infrastructure.Options;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Services;
using Edumination.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core MySQL
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(cs, ServerVersion.AutoDetect(cs)).UseSnakeCaseNamingConvention());

// Options
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth"));

builder.Services
    .AddOptions<AppOptions>()
    .Bind(builder.Configuration.GetSection("App"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.FrontendBaseUrl), "App:FrontendBaseUrl is required")
    .ValidateOnStart();

// MVC + FluentValidation
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

// Authentication & Authorization
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.TokenValidationParameters = new()
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Auth:Issuer"],
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

// DI Services
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IAuditLogger, AuditLogger>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IAssetService, AssetsService>();
builder.Services.AddScoped<IStorageService, StorageService>();

// Email sender (mock dev)
builder.Services.AddScoped<IEmailSender, MockEmailSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Edumination v1"));
}

// app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.Run();

public class MockEmailSender : IEmailSender
{
    public Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        Console.WriteLine($"[MOCK EMAIL] To:{to} | Subject:{subject}\n{htmlBody}");
        return Task.CompletedTask;
    }
}

public sealed class AppOptions
{
    public string FrontendBaseUrl { get; set; } = "";
}
