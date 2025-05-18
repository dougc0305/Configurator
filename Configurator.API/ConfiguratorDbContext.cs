using Common;
using Common.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.RegularExpressions;

public class ConfiguratorDbContext : DbContext
{
    public static string Schema = "configurator";
    public bool convertNamesToLower = true;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public int defaultUserId = -1;

    public ConfiguratorDbContext(DbContextOptions<ConfiguratorDbContext> options,
        IHttpContextAccessor? httpContextAccessor = null) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public static string DbConnectionString = "Timeout=180;Include Error Detail=true;Persist Security Info=True;Host=localhost;CommandTimeout=300;Username=postgres;Password=password;Database=configurator;Port=5332;SearchPath=ct,public";

    public DbSet<GearSpec> GearSpecs { get; set; }
    public DbSet<WormGearOperator> WormGearOperators { get; set; }
    public DbSet<SamboGear> SamboGears { get; set; }
    public DbSet<SamboGearMountingBase> SamboGearMountingBases { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Always call the base method last

        // Set the default schema for all tables in this context
        builder.HasDefaultSchema("configurator");

        if (convertNamesToLower)
        {
            foreach (var table in builder.Model.GetEntityTypes())
            {
                ConvertToLower(table);
                foreach (var property in table.GetProperties())
                {
                    ConvertToLower(property);
                }

                foreach (var primaryKey in table.GetKeys())
                {
                    ConvertToLower(primaryKey);
                }

                foreach (var foreignKey in table.GetForeignKeys())
                {
                    ConvertToLower(foreignKey);
                }

                foreach (var indexKey in table.GetIndexes())
                {
                    ConvertToLower(indexKey);
                }
            }
        }
    }

    private static readonly Regex _keysRegex = new Regex("^(PK|FK|IX)_", RegexOptions.Compiled);
    internal static void MakeNamesLowercase(ModelBuilder modelBuilder, bool useShortNames = false)
    {
        foreach (var table in modelBuilder.Model.GetEntityTypes())
        {
            ConvertToLower(table);
            foreach (var property in table.GetProperties())
            {
                ConvertToLower(property);
            }

            foreach (var primaryKey in table.GetKeys())
            {
                ConvertToLower(primaryKey);
            }

            foreach (var foreignKey in table.GetForeignKeys())
            {
                ConvertToLower(foreignKey, useShortNames);
            }

            foreach (var indexKey in table.GetIndexes())
            {
                ConvertToLower(indexKey);
            }
        }
    }


    private static void ConvertToLower(object entity, bool useShortNames = false)
    {
        switch (entity)
        {
            case IMutableEntityType table:
                string tableName = ConvertGeneralToLower(table.GetTableName());
                table.SetTableName(tableName);
                if (tableName.StartsWith("asp_net_"))
                {
                    table.SetTableName(tableName.Replace("asp_net_", string.Empty));
                    table.SetSchema("identity");
                }
                break;
            case IMutableProperty property:
                property.SetColumnName(ConvertGeneralToLower(property.GetColumnName(StoreObjectIdentifier.Table(property.DeclaringEntityType.GetTableName(), "configurator"))));
                break;
            case IMutableKey primaryKey:
                primaryKey.SetName(ConvertKeyToLower(primaryKey.GetName()));
                break;
            case IMutableForeignKey foreignKey:
                foreignKey.SetConstraintName(ConvertKeyToLower(foreignKey.GetConstraintName()));
                break;
            case IMutableIndex indexKey:
                indexKey.SetDatabaseName(ConvertKeyToLower(indexKey.GetDatabaseName()));
                break;
            default:
                throw new NotImplementedException("Unexpected type was provided to lower case converter");
        }
    }

    private static string ConvertKeyToLower(string keyName) =>
        ConvertGeneralToLower(_keysRegex.Replace(keyName, match => match.Value.ToLower()));

    private static string ConvertGeneralToLower(string entityName) =>
        entityName.ToLower();

    internal static void ApplyBaseEntityRules(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(t => t.ClrType.GetInterfaces().Any(i => i == typeof(IChangeTrackerDates) || i == typeof(IChangeTrackerNullableDates))))
        {
            modelBuilder.Entity(
                entityType.Name,
                x =>
                {
                    //x.Property("Id").ValueGeneratedOnAdd();

                    x.Property("CreatedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    x.Property("ModifiedDate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .ValueGeneratedOnAdd();
                });
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var baseColList = entityType.ClrType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
              .Where(i => (i.PropertyType == typeof(DateTime) || i.PropertyType == typeof(DateTime?))
                            && i.GetCustomAttributes(typeof(NotMappedAttribute), false).Length == 0
                            && i.GetCustomAttributes(typeof(ColumnAttribute), false).Length == 0)
              .OrderBy(o => o.Name).Select(s => s).ToList();

            string timestampType = "timestamp";

            foreach (var f in baseColList)
            {
                modelBuilder.Entity(
                    entityType.Name,
                    x =>
                    {
                        x.Property(f.Name).HasColumnType(timestampType);
                    });
            }
        }
    }

    public void SetTimeout(double? timeout)
    {
        Database.SetCommandTimeout((int?)timeout);
    }
    public void SetTimeout(TimeSpan timeout)
    {
        Database.SetCommandTimeout(timeout);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        AddTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        AddTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public async Task SetDefaultUserId()
    {
        //var employee = await this.Employees
        //        .Include(e => e.SystemUser)
        //        .FirstOrDefaultAsync(e => e.SystemUser.UserName == "systemUser");
        //defaultUserId = employee?.Id ?? -1;
    }
    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries().Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified)).ToList();
        if (entities.Any())
        {
            var userId = defaultUserId.ToString();
            try
            {
                var user = _httpContextAccessor?.HttpContext?.User;
                //var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                userId = user?.FindFirst("EmployeeId")?.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            foreach (var entity in entities)
            {
                IChangeTracker ict = entity.Entity as IChangeTracker;
                if (ict != null)
                {
                    ict.ModifiedDate = DateTime.Now;
                    if (ict.ModifiedById == 0)
                        ict.ModifiedById = userId != null ? int.Parse(userId) : defaultUserId;

                    if (entity.State == EntityState.Added)
                    {
                        if (ict.CreatedById == 0)
                            ict.CreatedById = userId != null ? int.Parse(userId) : defaultUserId;
                        ict.CreatedDate = DateTime.Now;
                    }
                }
            }
        }
    }

}


