using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Configuration
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasData
            (
                new Review
                {
                    Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
                    Heading = "Chemistry and life",
                    Text = "review text1",
                    Raiting = 9,
                    ProductId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                },
                new Review
                {
                    Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"),
                    Heading = "Walt still has time",
                    Text = "review text2",
                    Raiting = 8,
                    ProductId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                }
            );
        }
    }
}