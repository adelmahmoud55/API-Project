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
    internal class BasedAuditableEntityInterceptor : SaveChangesInterceptor // SaveChangesInterceptor is a class that allows you to intercept and modify the behavior of the SaveChanges method in Entity Framework Core. You can use this class to add custom logic before or after the SaveChanges method is called, or to cancel the operation entirely.
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public BasedAuditableEntityInterceptor(ILoggedInUserService loggedInUserService )
        {
            _loggedInUserService = loggedInUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            return base.SavingChanges(eventData, result);
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

            foreach (var entry in dbContext.ChangeTracker.Entries<BaseAuditableEntity<int>>()) // any entity iherit from BaseAuditableEntity must be authinticated , {there is a user already } 
            {
                if (entry is { State: EntityState.Added or EntityState.Modified })
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = _loggedInUserService.UserId!; // we ignore the nullability of the UserId property here because we know that the user is authenticated, lakn munf3sh a2ol _loggedInUserService.UserId ?? "" , l2n mfrod ykon fe user id
                        entry.Entity.CreatedOn = utcNow;
                    }
                    entry.Entity.LastModifiedBy = _loggedInUserService.UserId!;
                    entry.Entity.LastModifiedOn = utcNow;
                }
            }
        }
    }
}
