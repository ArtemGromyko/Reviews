using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public ReviewRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }
    }
}
