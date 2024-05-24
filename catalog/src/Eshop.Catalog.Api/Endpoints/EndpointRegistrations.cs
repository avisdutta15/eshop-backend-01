using Eshop.Catalog.Api.Endpoints.Catalog;
using Eshop.Catalog.Api.Endpoints.WeatherForecast;

namespace Eshop.Catalog.Api.Endpoints;

public static class EndpointRegistrations
{
    public static WebApplication UseCatalogEndpoints(this WebApplication app)
    {
        app.UseWeatherEndpoints();
        app.UseCatalogTypesEndpoints();

        return app;
    }

    public static WebApplication UseWeatherEndpoints(this WebApplication app)
    {
        var weatherForecastGroup = app.MapGroup("/weatherforecast");

        weatherForecastGroup.MapGet("/", GetWeatherForecast.Handle)
                            .WithName("GetWeatherForecast")
                            .WithOpenApi();
        return app;
    }

    public static WebApplication UseCatalogTypesEndpoints(this WebApplication app)
    {
        var catalogGroup = app.MapGroup("/catalog-types");

        catalogGroup.MapGet("/", GetCatalogTypes.Handle)
                    .WithName("GetCatalogTypes")
                    .WithOpenApi();

        return app;
    }
}
