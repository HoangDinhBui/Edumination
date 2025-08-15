using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Edumination.Api.Infrastructure.Persistence;
using Edumination.Api.Features.Papers.Services;
using Edumination.Api.Features.Attempts.Services;

namespace Edumination.Api.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppPersistence(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("Default");
        services.AddDbContext<AppDbContext>(o => o.UseMySql(cs, ServerVersion.AutoDetect(cs)));
        return services;
    }

    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration cfg)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o => o.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = cfg["Jwt:Issuer"],
                ValidAudience = cfg["Jwt:Audience"],
                IssuerSigningKey = key
            });
        services.AddAuthorization();
        return services;
    }

    public static IServiceCollection AddFeatureServices(this IServiceCollection services)
    {
        services.AddScoped<Features.Papers.Services.IPaperService, Features.Papers.Services.PaperService>();
        services.AddScoped<Features.Attempts.Services.IAttemptService, Features.Attempts.Services.AttemptService>();
        return services;
    }
}
