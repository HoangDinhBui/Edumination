using Edumination.Api.Common.Services;
using Edumination.Api.Features.Auth;
using Edumination.Api.Features.Auth.Requests;
using Edumination.Api.Features.Auth.Services;
using Edumination.Api.Infrastructure.External.Email;
using Edumination.Api.Infrastructure.Options;
using Edumination.Api.Infrastructure.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Edumination.Api.Features.Leaderboard.Interfaces;
using Edumination.Api.Features.Leaderboard.Services;

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

// DI Services
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IAuditLogger, AuditLogger>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Email sender (mock dev)
builder.Services.AddScoped<IEmailSender, MockEmailSender>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Edumination v1"));
}

// Áp dụng mọi migration còn thiếu khi khởi động (bật/tắt bằng cấu hình)
if (app.Configuration.GetValue<bool>("Database:MigrateOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate(); // hoặc await db.Database.MigrateAsync();
        Console.WriteLine("[DB] Migrations applied successfully.");
        // (tùy chọn) Seed dữ liệu:
        // await SeedData.RunAsync(db);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"[DB] Migration failed: {ex.Message}");
        throw; // cho app fail fast nếu migrate lỗi
    }
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
