using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                        .HasMany(p => p.Actors)
                        .WithMany(p => p.ProductsActor)
                        .UsingEntity(j => j.ToTable("ProductActor"));

            modelBuilder.Entity<Product>()
                        .HasMany(p => p.Directors)
                        .WithMany(p => p.ProductsDirector)
                        .UsingEntity(j => j.ToTable("ProductDirector"));
        }
    }
}
