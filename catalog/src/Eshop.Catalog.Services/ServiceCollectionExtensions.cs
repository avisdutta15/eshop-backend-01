namespace Eshop.Catalog.Services; 

using Azure.Core;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

public static class ServiceCollectionExtensions
{
    private static readonly DefaultAzureCredential AzureCredentials = new();
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
                return accessToken.Token;
            }, TimeSpan.FromMinutes(55), TimeSpan.FromSeconds(5))
            .Build();
    }
}
