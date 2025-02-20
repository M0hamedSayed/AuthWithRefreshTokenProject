using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Helpers
{
    public sealed class SaveChangesInterceptor : Microsoft.EntityFrameworkCore.Diagnostics.SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
            {
                return base.SavingChangesAsync(
                    eventData, result, cancellationToken);
            }

            // soft delete

            IEnumerable<EntityEntry<ISoftDeletable>> entries =
                eventData
                    .Context
                    .ChangeTracker
                    .Entries<ISoftDeletable>()
                    .Where(e => e.State == EntityState.Deleted);

            foreach (EntityEntry<ISoftDeletable> softDeletable in entries)
            {
                softDeletable.State = EntityState.Modified;
                softDeletable.Entity.IsDeleted = true;
                softDeletable.Entity.DeletedAt = DateTime.UtcNow;
            }

            // handle updated at
            IEnumerable<EntityEntry<ITableCreation>> updatedEntries =
                eventData
                    .Context
                    .ChangeTracker
                    .Entries<ITableCreation>()
                    .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (EntityEntry<ITableCreation> entity in updatedEntries)
            {
                var now = DateTime.UtcNow; // current datetime

                if (entity.State == EntityState.Added)
                {
                    ((ITableCreation)entity.Entity).CreatedAt = now;
                }
                ((ITableCreation)entity.Entity).UpdatedAt = now;
            }


            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
