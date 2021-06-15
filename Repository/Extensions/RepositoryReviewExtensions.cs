using Entities.Models;
using System.Linq;

namespace Repository.Extensions
{
    public static class RepositoryReviewExtensions
    {
        public static IQueryable<Review> FilterReviews(this IQueryable<Review> reviews, uint minRaiting, uint maxRaiting) =>
            reviews.Where(r => (r.Raiting >= minRaiting && r.Raiting <= maxRaiting));

        public static IQueryable<Review> Search(this IQueryable<Review> reviews, string searchTearm)
        {
            if (string.IsNullOrWhiteSpace(searchTearm))
                return reviews;

            var lowerCaseTerm = searchTearm.Trim().ToLower();

            return reviews.Where(r => r.Heading.ToLower().Contains(lowerCaseTerm));
        }
    }
}
