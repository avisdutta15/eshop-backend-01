namespace Eshop.Catalog.Data.Entities.Domain;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class CatalogTypeEntity
{
    public int Id { get; set; }

    [Required]
    public string Type { get; set; } = string.Empty;

    public class Configuration : IEntityTypeConfiguration<CatalogTypeEntity>
    {
        public void Configure(EntityTypeBuilder<CatalogTypeEntity> builder)
        {
            builder.ToTable("catalog_types", CatalogContextConstants.Schemas.Domain);
            builder.HasKey(e => e.Id);
            builder.HasData(
                new { Id = 1, Type = "T-Shirt" },
                new { Id = 2, Type = "Pant" }
            );
        }
    }
}
