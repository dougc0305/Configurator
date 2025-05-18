using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace BusinessSystem.Server
{
    public class DbContextFactories
    {
        public class CTContextFactory : IDesignTimeDbContextFactory<ConfiguratorDbContext>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CTContextFactory()
            {
            }

            public CTContextFactory(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public ConfiguratorDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ConfiguratorDbContext>()
                    .UseNpgsql(ConfiguratorDbContext.DbConnectionString,
                        o => o.CommandTimeout((int)TimeSpan.FromMinutes(1).TotalSeconds)
                        .EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null)
                        .MigrationsHistoryTable("__EFMigrationsHistory", ConfiguratorDbContext.Schema));

                return new ConfiguratorDbContext(optionsBuilder.Options, _httpContextAccessor);
            }
        }
    }
}
