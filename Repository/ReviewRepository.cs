using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public ReviewRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<PagedList<Review>> GetReviewsAsync(Guid productId, ReviewParameters reviewParameters, bool trackChanges)
        {
            var reviews = await FindByCondition(p => p.ProductId.Equals(productId), trackChanges)
            .ToListAsync();

            return PagedList<Review>.ToPagedList(reviews, reviewParameters.PageNumber, reviewParameters.PageSize);
        }

        public async Task<Review> GetReviewAsync(Guid productId, Guid id, bool trackChanges) =>
            await FindByCondition(p => p.Id.Equals(id) && p.ProductId.Equals(productId), trackChanges).SingleOrDefaultAsync();

        public void DeleteReview(Review review) => Delete(review);

        public void CreateReviewForProduct(Guid productId, Review review)
        {
            review.ProductId = productId;
            Create(review);
        }

        public async Task<IEnumerable<Review>> GetByIdsAsync(Guid productId, IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(r => r.ProductId.Equals(productId) && ids.Contains(r.Id), trackChanges).ToListAsync();
    }
}
