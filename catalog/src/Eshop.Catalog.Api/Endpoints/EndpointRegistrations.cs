using Eshop.Catalog.Api.Endpoints.WeatherForecast;

namespace Eshop.Catalog.Api.Endpoints;

public static class EndpointRegistrations
{
    public static WebApplication UseCatalogEndpoints(this WebApplication app)
    {
        app.UseWeatherEndpoints();
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
}
