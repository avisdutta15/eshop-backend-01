using Eshop.Catalog.Api.Endpoints;
using Eshop.Catalog.Data;
using Eshop.Catalog.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAADPostgresDbContext<CatalogContext>(builder.Configuration.GetConnectionString("Catalog"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Configuration.GetValue("EnableSwagger", false))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDeveloperExceptionPage();
app.UseCatalogEndpoints();

app.Run();


