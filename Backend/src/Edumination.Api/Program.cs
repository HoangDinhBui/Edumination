using Education.Repositories;
using Education.Repositories.Interfaces;
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
using Edumination.Api.Features.Papers.Services;
using Edumination.Api.Features.Admin.Services;
using Edumination.Api.Features.Courses.Services;
using Edumination.Api.Repositories.Interfaces;
using Edumination.Api.Repositories;
using Edumination.Api.Services.Interfaces;
using Edumination.Persistence.Repositories;
using System.Security.Claims;
using Edumination.Services;
using Edumination.Domain.Interfaces;
using Edumination.Persistence;
using Edumination.Api.Features.Admin.Validators;
using Edumination.Api.Features.Courses.Dtos;
using Edumination.Api.Features.Courses.Validators;
using Edumination.Api.Infrastructure.Persistence.Repositories;
using Edumination.Api.Features.Enrollments.Services;
using Edumination.Api.Features.Stats.Services;
using Edumination.Api.Features.Recommendations.Services;
using Edumination.Api.Features.Attempts.Services;
using Stripe;
using Microsoft.AspNetCore.Cors;
using MongoDB.Driver;
using Edumination.Api.Domain.MongoEntities;
using Edumination.Api.Features.Courses.Services;

var builder = WebApplication.CreateBuilder(args);

// --- CẤU HÌNH MONGODB ---
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDbConnection");
// Mặc định là english_learning_logs nếu không tìm thấy trong config
var mongoDbName = builder.Configuration["MongoDbSettings:DatabaseName"] ?? "english_learning_logs";

// --- CẤU HÌNH MYSQL ---
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

// --- ĐĂNG KÝ DỊCH VỤ MONGODB ---
// 1. Đăng ký Client
builder.Services.AddSingleton<IMongoClient>(sp => 
    new MongoClient(mongoConnectionString));

// 2. Đăng ký Database (THAY ĐỔI QUAN TRỌNG Ở ĐÂY)
// Inject IMongoDatabase để Service có thể linh động chọn Collection (Reading/Listening)
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDbName);
});

builder.Services
    .AddOptions<AppOptions>()
    .Bind(builder.Configuration.GetSection("App"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.FrontendBaseUrl), "App:FrontendBaseUrl is required")
    .ValidateOnStart();

// MVC + FluentValidation + Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

// Stripe Configuration
var stripeKey = builder.Configuration["Stripe:SecretKey"];
if (string.IsNullOrWhiteSpace(stripeKey))
{
    // Warning thay vì Throw để dev không có key vẫn chạy được (tùy chọn)
    Console.WriteLine("WARNING: Stripe:SecretKey is missing.");
}
else 
{
    builder.Services.AddSingleton<IStripeClient>(new StripeClient(stripeKey));
}

// Services DI
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
builder.Services.AddScoped<IValidator<UpdateCourseRequest>, UpdateCourseRequestValidator>();
builder.Services.AddScoped<IValidator<CreateModuleRequest>, CreateModuleRequestValidator>();
builder.Services.AddScoped<IPassageRepository, PassageRepository>();
builder.Services.AddScoped<IPassageService, PassageService>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IQuestionChoiceService, QuestionChoiceService>();
builder.Services.AddScoped<IQuestionChoiceRepository, QuestionChoiceRepository>();
builder.Services.AddScoped<IUserStatsService, UserStatsService>();
builder.Services.AddScoped<ICourseRecommendationService, CourseRecommendationService>();
builder.Services.AddScoped<IMyEnrollmentsService, MyEnrollmentsService>();
builder.Services.AddScoped<ITestAttemptRepository, TestAttemptRepository>();
builder.Services.AddScoped<ISectionAttemptRepository, SectionAttemptRepository>();
builder.Services.AddScoped<IAttemptService, AttemptService>();
builder.Services.AddScoped<IModuleService, ModuleService>();


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

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var frontendUrl = builder.Configuration["App:FrontendBaseUrl"];
        policy.WithOrigins("http://localhost:8082", frontendUrl ?? "")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddHttpClient();
// Groq AI settings
builder.Services.Configure<Edumination.Api.Infrastructure.Services.GroqApiSettings>(builder.Configuration.GetSection("Groq"));
builder.Services.AddScoped<Edumination.Api.Features.Attempts.Services.ISpeakingGradingService, Edumination.Api.Infrastructure.Services.GroqSpeakingGradingService>();

// DI Services
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IAuditLogger, AuditLogger>();
builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<IStorageService, StorageService>(); // Đã add ở trên rồi

// Email sender
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IEmailSender, MockEmailSender>();
    Console.WriteLine("[EMAIL] Using MockEmailSender for Development");
}
else
{
    builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
    Console.WriteLine("[EMAIL] Using SmtpEmailSender for Production");
}

builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

// Bind SMTP options
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Edumination v1"));
}

// Middleware debug
app.Use(async (context, next) =>
{
    Console.WriteLine($"[MIDDLEWARE] Request Path: {context.Request.Path}, Content-Type: {context.Request.ContentType}");
    await next.Invoke();
});

// CORS
app.UseCors();

// DB Migrations
if (app.Configuration.GetValue<bool>("Database:MigrateOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
        Console.WriteLine("[DB] Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"[DB] Migration failed: {ex.Message}");
        // throw; // Có thể comment throw để không crash app nếu migration lỗi
    }
}

// Log routes
var ep = app.Services.GetRequiredService<EndpointDataSource>();
// foreach (var e in ep.Endpoints.OfType<RouteEndpoint>())
//    Console.WriteLine($"[ROUTE] {e.RoutePattern.RawText}");

// Override Google OAuth from env
var envClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
if (!string.IsNullOrWhiteSpace(envClientId))
{
    builder.Configuration["OAuth:Google:ClientId"] = envClientId;
    Console.WriteLine("[GOOGLE] ClientId loaded from env.");
}

var envClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
if (!string.IsNullOrWhiteSpace(envClientSecret))
{
    builder.Configuration["OAuth:Google:ClientSecret"] = envClientSecret;
    Console.WriteLine("[GOOGLE] ClientSecret loaded from env.");
}

// Override Stripe from env
var envStripeKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
if (!string.IsNullOrWhiteSpace(envStripeKey))
{
    Console.WriteLine("[STRIPE] Key loaded from env.");
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }));
app.Run();

// MockEmailSender
public class MockEmailSender : IEmailSender
{
    private readonly ILogger<MockEmailSender> _logger;

    public MockEmailSender(ILogger<MockEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        _logger.LogInformation("========== MOCK EMAIL ==========");
        _logger.LogInformation("To: {To}", to);
        _logger.LogInformation("Subject: {Subject}", subject);
        // _logger.LogInformation("Body:\n{Body}", htmlBody); // Comment bớt body cho đỡ rác log
        _logger.LogInformation("================================");

        var emailsDir = Path.Combine(Directory.GetCurrentDirectory(), "mock_emails");
        Directory.CreateDirectory(emailsDir);

        var fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{subject.Replace(" ", "_")}.html";
        var filePath = Path.Combine(emailsDir, fileName);

        System.IO.File.WriteAllText(filePath, $@"
<!DOCTYPE html>
<html>
<head><meta charset='UTF-8'><title>{subject}</title></head>
<body>
    <h3>Mock Email Preview</h3>
    <p><strong>To:</strong> {to}</p>
    <p><strong>Subject:</strong> {subject}</p>
    <hr>
    {htmlBody}
</body>
</html>");

        _logger.LogInformation("Email saved to: {FilePath}", filePath);
        return Task.CompletedTask;
    }
}

// AppOptions
public sealed class AppOptions
{
    public string FrontendBaseUrl { get; set; } = "";
}