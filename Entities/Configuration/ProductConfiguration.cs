using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData
            (
                new Product
                {
                    Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    Name = "Breaking Bad",
                    Slogan = "In the no-holds-barred world of Walt White, the end justifies the extreme",
                    ReleaseDate = new DateTime(2008, 01, 20),
                    Country = "USA",
                    Genre = "thriller, crime, dramas",
                    Category = "series"
                }
            );

            builder.HasMany(p => p.Actors)
                    .WithMany(p => p.ProductsActor)
                    .UsingEntity<Dictionary<string, object>>
                    (
                        "ActorsProducts",
                        r => r.HasOne<Person>().WithMany().HasForeignKey("PersonId"),
                        l => l.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                        je =>
                        {
                            je.HasKey("PersonId", "ProductId");
                            je.HasData
                            (
                                new { PersonId = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), ProductId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870") },
                                new { PersonId = new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"), ProductId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870") }
                            );
                        }
                    );

            builder.HasMany(p => p.Directors)
                    .WithMany(p => p.ProductsDirector)
                    .UsingEntity<Dictionary<string, object>>
                    (
                        "DirectorsProducts",
                        r => r.HasOne<Person>().WithMany().HasForeignKey("PersonId"),
                        l => l.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                        je =>
                        {
                            je.HasKey("PersonId", "ProductId");
                            je.HasData
                            (
                                new { PersonId = new Guid("53a1237a-3ed3-4462-b9f0-5a7bb1056a33"), ProductId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870") }
                            );
                        }
                    );
        }
    }
}

