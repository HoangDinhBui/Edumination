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
using Edumination.Api.Features.Leaderboard.Interfaces;
using Edumination.Api.Features.Leaderboard.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Edumination.Services;
using Edumination.Domain.Interfaces;
using Edumination.Persistence;
using Edumination.Api.Features.Papers.Services;
using Microsoft.AspNetCore.Routing;
using Edumination.Api.Features.Admin.Services;
using Edumination.Api.Features.Admin.Validators;
using Edumination.Api.Features.Courses.Services;
using Edumination.Api.Repositories.Interfaces;
using Edumination.Api.Repositories;
using Edumination.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// EF Core MySQL
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(cs, serverVersion)
       .UseSnakeCaseNamingConvention());

// Options
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.Configure<OAuthOptions>(builder.Configuration.GetSection("OAuth"));
builder.Services.AddMemoryCache();


builder.Services
    .AddOptions<AppOptions>()
    .Bind(builder.Configuration.GetSection("App"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.FrontendBaseUrl), "App:FrontendBaseUrl is required")
    .ValidateOnStart();

// MVC + FluentValidation + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
builder.Services.AddScoped<IAssetService, AssetsService>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IVirusScanner, VirusScanner>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProfileRequestValidator>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPaperService, PaperService>();
builder.Services.Configure<GoogleOAuthOptions>(builder.Configuration.GetSection("OAuth:Google"));
builder.Services.AddHttpClient<IGoogleOAuthClient, GoogleOAuthClient>();
builder.Services.AddScoped<IOAuthService, OAuthService>();
builder.Services.AddScoped<IAdminUsersService, AdminUsersService>();
builder.Services.AddScoped<IEduDomainService, EduDomainService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEduDomainRequestValidator>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<ISectionService, SectionService>();

// Authentication & Authorization
var jwt = builder.Configuration.GetSection("Jwt");
var jwtKey = jwt.GetValue<string>("Key") ?? "";
var jwtIssuer = jwt.GetValue<string>("Issuer");
var jwtAudience = jwt.GetValue<string>("Audience");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // dev
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2),
            RoleClaimType = ClaimTypes.Role
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddHttpClient();

// DI Services
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IAuditLogger, AuditLogger>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IAssetService, AssetsService>();
builder.Services.AddScoped<IStorageService, StorageService>();

// Email sender (mock dev)
builder.Services.AddScoped<IEmailSender, MockEmailSender>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

// Options
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Bind SMTP options
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

// Chọn implementation theo môi trường
if (builder.Environment.IsDevelopment())
    builder.Services.AddScoped<IEmailSender, MockEmailSender>();
else
    builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

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


var ep = app.Services.GetRequiredService<EndpointDataSource>();
foreach (var e in ep.Endpoints.OfType<RouteEndpoint>())
    Console.WriteLine($"[ROUTE] {e.RoutePattern.RawText}");

var envClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
if (!string.IsNullOrWhiteSpace(envClientId))
    builder.Configuration["OAuth:Google:ClientId"] = envClientId;

var envClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
if (!string.IsNullOrWhiteSpace(envClientSecret))
    builder.Configuration["OAuth:Google:ClientSecret"] = envClientSecret;

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

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
