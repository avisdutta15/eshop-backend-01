namespace Entity.Data.Migrations; 

using Eshop.Catalog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class CatalogContextFactory : IDesignTimeDbContextFactory<CatalogContext>
{
    public CatalogContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "Configurations", "appsettings.json"), true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
            .UseNpgsql(configuration.GetConnectionString("Catalog"), sql =>
            {
                sql.MigrationsHistoryTable("__efmigrations_catalog");
                sql.MigrationsAssembly(typeof(CatalogContextFactory).Assembly.FullName);
            });

        return new CatalogContext(optionsBuilder.Options);
    }
}
