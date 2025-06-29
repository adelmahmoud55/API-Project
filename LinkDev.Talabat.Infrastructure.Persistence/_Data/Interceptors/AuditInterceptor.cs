using LinkDev.Talabat.Core.Application.Abstaction;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors
{
    internal class AuditInterceptor : SaveChangesInterceptor // SaveChangesInterceptor is a class that allows you to intercept and modify the behavior of the SaveChanges method in Entity Framework Core. You can use this class to add custom logic before or after the SaveChanges method is called, or to cancel the operation entirely.
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public AuditInterceptor(ILoggedInUserService loggedInUserService )
        {
            _loggedInUserService = loggedInUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }


        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context); // we update the entities before saving changes to the database
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        private void UpdateEntities(DbContext? dbContext)
        {
            if (dbContext is null) 
                return;
            var entries = dbContext.ChangeTracker.Entries<IBaseAuditableEntity>()
                           .Where(entity => entity.State is EntityState.Added or EntityState.Modified);





           
            foreach (var entry in entries) // any entity iherit from BaseAuditableEntity must be authinticated , {there is a user already } 
            {
                //if (entry.Entity is Order or OrderItem)
                //    _loggedInUserService.UserId = "";


                if(string.IsNullOrEmpty(_loggedInUserService.UserId))
                _loggedInUserService.UserId = "webhook"; // if the user is not authenticated, we set the UserId to "System" to indicate that the changes were made by the system


                if (entry is { State: EntityState.Added or EntityState.Modified })
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = _loggedInUserService.UserId!; // we ignore the nullability of the UserId property here because we know that the user is authenticated, lakn munf3sh a2ol _loggedInUserService.UserId ?? "" , l2n mfrod ykon fe user id
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                    }
                    entry.Entity.LastModifiedBy = _loggedInUserService.UserId!;
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
