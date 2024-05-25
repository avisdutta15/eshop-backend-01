namespace Eshop.Catalog.Services; 

using Azure.Core;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

public static class ServiceCollectionExtensions
{
    //Source: https://learn.microsoft.com/en-us/azure/app-service/tutorial-connect-msi-azure-database?tabs=postgresql-sc%2Cuserassigned-sc%2Cdotnet%2Cdotnet-mysql-mi%2Cdotnet-postgres-mi%2Cwindowsclient#3-modify-your-code
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

    public static IServiceCollection AddAADPostgresDbContext<TContext>(this IServiceCollection services, string? connectionString)
        where TContext : DbContext
    {
        var dataSource = GetDataSource(connectionString);

        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(dataSource);
        });

        return services;
    }

    private static NpgsqlDataSource GetDataSource(string? connectionString)
    {
        return new NpgsqlDataSourceBuilder(connectionString)
            .UsePeriodicPasswordProvider(async (builder, ct) =>
            {
                var accessToken = await AzureCredentials.GetTokenAsync(OssrdbmsTokenRequest, ct);
                Console.WriteLine(accessToken.Token);
                return accessToken.Token;
            }, TimeSpan.FromMinutes(55), TimeSpan.FromSeconds(5))
            .Build();
    }
}
