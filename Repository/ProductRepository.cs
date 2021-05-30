using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public IEnumerable<Product> GetAllProducts(bool trackChanges) =>
            FindAll(trackChanges)
            .Include(p => p.Actors)
            .Include(p => p.Directors)
            .ToList();

        public Product GetProduct(Guid productid, bool trackChanges) =>
            FindAll(trackChanges)
            .Include(p => p.Actors)
            .Include(p => p.Directors)
            .Where(p => p.Id.Equals(productid))
            .SingleOrDefault();

        public void CreateProduct(Product product) => Create(product);
    }
}
