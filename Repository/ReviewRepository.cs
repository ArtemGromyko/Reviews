﻿using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public ReviewRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public IEnumerable<Review> GetReviews(Guid productId, bool trackChanges) =>
            FindByCondition(p => p.ProductId.Equals(productId), trackChanges).ToList();

        public Review GetReview(Guid productId, Guid id, bool trackChanges) =>
            FindByCondition(p => p.Id.Equals(id) && p.ProductId.Equals(productId), trackChanges).SingleOrDefault();

        public void DeleteReview(Review review) => Delete(review);

        public void CreateReviewForProduct(Guid productId, Review review)
        {
            review.ProductId = productId;
            Create(review);
        }

        public IEnumerable<Review> GetByIds(Guid productId, IEnumerable<Guid> ids, bool trackChanges) =>
            FindByCondition(r => r.ProductId.Equals(productId) && ids.Contains(r.Id), trackChanges).ToList();
    }
}
