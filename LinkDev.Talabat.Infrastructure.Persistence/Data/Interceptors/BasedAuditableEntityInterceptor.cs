using LinkDev.Talabat.Core.Application.Abstaction;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors
{
    internal class BasedAuditableEntityInterceptor : SaveChangesInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public BasedAuditableEntityInterceptor(ILoggedInUserService loggedInUserService )
        {
            _loggedInUserService = loggedInUserService;
        }


        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }


        private void UpdateEntities(DbContext? dbContext)
        {
            if (dbContext is null) return;

            var utcNow = DateTime.UtcNow;

            foreach (var entry in dbContext.ChangeTracker.Entries<BaseAuditableEntity<int>>())
            {
                if (entry is { State: EntityState.Added or EntityState.Modified })
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = "";
                        entry.Entity.CreatedOn = utcNow;
                    }
                    entry.Entity.LastModifiedBy = "";
                    entry.Entity.LastModifiedOn = utcNow;
                }
            }
        }
    }
}
