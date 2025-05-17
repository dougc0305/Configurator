using Configurator.API.DomainObjects;
using Microsoft.EntityFrameworkCore;

public class ConfiguratorDbContext : DbContext
{
    public ConfiguratorDbContext(DbContextOptions<ConfiguratorDbContext> options) : base(options) { }

    public DbSet<GearSpec> GearSpecs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set the default schema for all tables in this context
        modelBuilder.HasDefaultSchema("configurator");

        base.OnModelCreating(modelBuilder); // Always call the base method last
    }
}


