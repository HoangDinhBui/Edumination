using Edumination.Api.Domain.Entities;
using Edumination.Api.Infrastructure.Persistence;
using System.Text.Json;

namespace Edumination.Api.Common.Services;

public interface IAuditLogger
{
    Task LogAsync(long? userId, string action, string? entityKind, long? entityId, object? data = null, CancellationToken ct = default);
}

public class AuditLogger(AppDbContext db) : IAuditLogger
{
    public async Task LogAsync(long? userId, string action, string? entityKind, long? entityId, object? data = null, CancellationToken ct = default)
    {
        var log = new AuditLog
        {
            UserId = userId,
            Action = action,
            EntityKind = entityKind,
            EntityId = entityId,
            DataJson = data is null ? null : JsonSerializer.Serialize(data)
        };
        db.AuditLogs.Add(log);
        await db.SaveChangesAsync(ct);
    }
}
