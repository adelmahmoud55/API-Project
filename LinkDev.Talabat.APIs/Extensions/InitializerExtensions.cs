using LinkDev.Talabat.Core.Domain.Contracts.Persistence;

namespace LinkDev.Talabat.APIs.Extensions
{
    public static class InitializerExtensions // static cuz it includes extension methods.
    {
        public static async Task<WebApplication> InitializerStoreContextAsync(this WebApplication app)
        {
            using var Scope = app.Services.CreateAsyncScope();
            var Services = Scope.ServiceProvider;

            var StoreContextInitializer = Services.GetRequiredService<IStoreContextInitializer>();
            // ask runtime enviroment  for an object from "StoreContext" Service explicitly.

            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {

                await StoreContextInitializer.InitializeAsync();
                await StoreContextInitializer.SeedAsync();

            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during applying the migratings or the data seeding.");
            }


            return app;
        }
    }
}
