using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .Include(p => p.Actors)
            .Include(p => p.Directors)
            .ToListAsync();

        public async Task<Product> GetProductAsync(Guid productid, bool trackChanges) =>
            await FindAll(trackChanges)
            .Include(p => p.Actors)
            .Include(p => p.Directors)
            .Where(p => p.Id.Equals(productid))
            .SingleOrDefaultAsync();

        public void CreateProduct(Product product) => Create(product);

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
    }
}
