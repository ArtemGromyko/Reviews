using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges);
        Task<Product> GetProductAsync(Guid productId, bool trackChanges);
        Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void CreateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
