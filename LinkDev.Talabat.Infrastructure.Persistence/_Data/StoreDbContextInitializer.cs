using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data
{
    public class StoreDbContextInitializer(StoreDbContext _dbContext) : IStoreContextInitializer
    {


        public async Task InitializeAsync()
        {
            var PendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(); // Get Pending Migrations.
                                                                                           //we can use MigrateAsync direct but time consuming for GetPendingMigrations is less time consuming than checking the database for pending migrations using MigrateAsync.



            if (PendingMigrations.Any())
                await _dbContext.Database.MigrateAsync(); // Update Database 
        }

        public async Task SeedAsync()
        {
            if (!_dbContext.Brands.Any())
            {
                var brandsData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0) /*brands is not null && brands.Count > 0*/
                {
                    await _dbContext.Set<ProductBrand>().AddRangeAsync(brands);
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (!_dbContext.Categories.Any())
            {
                var categoriesData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories?.Count > 0)
                {
                    await _dbContext.Set<ProductCategory>().AddRangeAsync(categories);
                    await _dbContext.SaveChangesAsync();
                }
            }


            if (!_dbContext.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count > 0)
                {
                    await _dbContext.Set<Product>().AddRangeAsync(products);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
