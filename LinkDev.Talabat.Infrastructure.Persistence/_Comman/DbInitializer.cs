using LinkDev.Talabat.Core.Domain.Contracts.Persistence.DbInitializer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Comman
{
    public abstract class DbInitializer(DbContext _dbContext) : IDbInitializer
    {
        public virtual async Task InitializeAsync()
        {
            var PendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(); // Get Pending Migrations.
                                                                                           //we can use MigrateAsync direct but time consuming for GetPendingMigrations is less time consuming than checking the database for pending migrations using MigrateAsync.



            if (PendingMigrations.Any())
                await _dbContext.Database.MigrateAsync(); // Update Database 
        }

        public abstract  Task SeedAsync();
       
    }
}
