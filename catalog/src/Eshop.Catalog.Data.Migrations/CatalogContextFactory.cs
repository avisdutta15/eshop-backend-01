namespace Entity.Data.Migrations; 

using Azure.Core;
using Azure.Identity;
using Eshop.Catalog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class CatalogContextFactory : IDesignTimeDbContextFactory<CatalogContext>
{
    //Source: https://learn.microsoft.com/en-us/azure/app-service/tutorial-connect-msi-azure-database?tabs=postgresql-sc%2Cuserassigned-sc%2Cdotnet%2Cdotnet-mysql-mi%2Cdotnet-postgres-mi%2Cwindowsclient#3-modify-your-code
    //        https://learn.microsoft.com/en-us/azure/app-service/tutorial-connect-msi-azure-database?tabs=sqldatabase-sc%2Cuserassigned-sc%2Cdotnet%2Cdotnet-mysql-mi%2Cdotnet-postgres-mi%2Cwindowsclient
    //        https://github.com/microsoft/azure-container-apps/issues/442#issuecomment-1846350428
    // Uncomment the following lines according to the authentication type.

    // For system-assigned identity.
    //private static readonly DefaultAzureCredential AzureCredentials = new();

    // For user-assigned identity.
    private static readonly DefaultAzureCredential AzureCredentials = new(new DefaultAzureCredentialOptions
    {
        ManagedIdentityClientId = Environment.GetEnvironmentVariable("AZURE_POSTGRESQL_CLIENTID")
    });

    private static readonly TokenRequestContext OssrdbmsTokenRequest = new(
    [
        "https://ossrdbms-aad.database.windows.net/.default"
    ]);

    public CatalogContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "Configurations", "appsettings.json"), true)
            .Build();

        var connectionString = configuration.GetConnectionString("Catalog");

        var datasource =  new NpgsqlDataSourceBuilder(connectionString)
                .UsePeriodicPasswordProvider(async (builder, ct) =>
                {
                    var accessToken = await AzureCredentials.GetTokenAsync(OssrdbmsTokenRequest, ct);
                    return accessToken.Token;
                }, TimeSpan.FromMinutes(55), TimeSpan.FromSeconds(5))
                .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>()
            .UseNpgsql(datasource, sql =>
            {
                sql.MigrationsHistoryTable("__efmigrations_catalog");
                sql.MigrationsAssembly(typeof(CatalogContextFactory).Assembly.FullName);
            });

        return new CatalogContext(optionsBuilder.Options);
    }
}
