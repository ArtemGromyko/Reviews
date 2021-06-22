using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Parameters;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<PagedList<Product>> GetAllProductsAsync(ProductParameters productParameters, bool trackChanges)
        {
            var products = await FindAll(trackChanges)
                .Include(p => p.Actors)
                .Include(p => p.Directors)
                .FilterProducts(productParameters.Categories, productParameters.Genres, productParameters.Countries)
                .Search(productParameters.SearchTerm)
                .Sort(productParameters.OrderBy)
                .AsSplitQuery()
                .ToListAsync();

            return PagedList<Product>.ToPagedList(products, productParameters.PageNumber, productParameters.PageSize);
        }

        public async Task<Product> GetProductAsync(Guid productid, bool trackChanges) =>
            await FindByCondition(p => p.Id.Equals(productid), trackChanges)
            .Include(p => p.Actors)
            .Include(p => p.Directors)
            .AsSplitQuery()
            .SingleOrDefaultAsync();

        public void CreateProduct(Product product) => Create(product);

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

        public void DeleteProduct(Product product) => Delete(product);
    }
}
