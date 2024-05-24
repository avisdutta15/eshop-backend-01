namespace Eshop.Catalog.Api.Endpoints.Catalog;

using Eshop.Catalog.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public static class GetCatalogTypes
{
    public static async Task<Results<NotFound,Ok<List<string>>>> Handle(CatalogContext catalogContext) 
    {
        var catalogTypes = await catalogContext.CatalogTypes.Select(x => x.Type).ToListAsync();

        if(catalogTypes == null)
            return TypedResults.NotFound();
        
        return TypedResults.Ok(catalogTypes);
    }
}
