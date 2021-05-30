using Entities.Models;
using System;
using System.Collections.Generic;

namespace Contracts
{
    public interface IReviewRepository
    {
        IEnumerable<Review> GetReviews(Guid productId, bool trackChanges);
        Review GetReview(Guid productId, Guid id, bool trackChanges);
        void CreateReviewForProduct(Guid productId, Review review);
        void DeleteReview(Review review);
    }
}
