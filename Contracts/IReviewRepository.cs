using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsAsync(Guid productId, bool trackChanges);
        Task<Review> GetReviewAsync(Guid productId, Guid id, bool trackChanges);
        Task<IEnumerable<Review>> GetByIdsAsync(Guid productId, IEnumerable<Guid> ids, bool trackChanges);
        void CreateReviewForProduct(Guid productId, Review review);
        void DeleteReview(Review review);
    }
}
