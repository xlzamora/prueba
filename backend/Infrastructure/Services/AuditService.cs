using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TelemedicinaOdonto.Application.Contracts;
using TelemedicinaOdonto.Application.DTOs;
using TelemedicinaOdonto.Domain.Entities;
using TelemedicinaOdonto.Infrastructure.Data;

namespace TelemedicinaOdonto.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly AppDbContext _dbContext;

    public AuditService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task RecordAsync(string actorUserId, string actorRole, string action, string entity, string entityId, object metadata)
    {
        var audit = new AuditLog
        {
            AuditId = Guid.NewGuid(),
            ActorUserId = actorUserId,
            ActorRole = actorRole,
            Action = action,
            Entity = entity,
            EntityId = entityId,
            Timestamp = DateTime.UtcNow,
            MetadataJson = JsonSerializer.Serialize(metadata)
        };

        _dbContext.AuditLogs.Add(audit);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<AuditLogResponse>> GetAllAsync()
    {
        return await _dbContext.AuditLogs
            .OrderByDescending(a => a.Timestamp)
            .Select(a => new AuditLogResponse(a.AuditId, a.ActorUserId, a.ActorRole, a.Action, a.Entity, a.EntityId, a.Timestamp, a.MetadataJson))
            .ToListAsync();
    }
}
