using Khuyaway.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Khuyaway.EntityFrameworkCore.Interceptors;

public class AuditableEntityInterceptor(
    ICurrentUser currentUser,
    TimeProvider timeProvider) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetAuditableData(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SetAuditableData(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetAuditableData(DbContext? context)
    {
        if (context is null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                var utcNow = timeProvider.GetUtcNow();

                if (entry.State is EntityState.Added)
                {
                    entry.Entity.CreatedBy = currentUser.Id;
                    entry.Entity.Created = utcNow;
                }

                if (entry.State is EntityState.Modified && entry.Entity.IsDeleted)
                {
                    entry.Entity.DeletedBy = currentUser.Id;
                    entry.Entity.Deleted = utcNow;
                }

                entry.Entity.LastModifiedBy = currentUser.Id;
                entry.Entity.LastModified = utcNow;
            }
        }
    }
}

internal static class Extensions
{
    internal static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}