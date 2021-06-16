using Entities.Models;
using Repository.Extensions.Utility;
using System.Linq;
using System.Linq.Dynamic.Core;

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

        public static IQueryable<Review> Sort(this IQueryable<Review> reviews, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return reviews.OrderBy(r => r.Heading);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Review>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return reviews.OrderBy(r => r.Heading);

            return reviews.OrderBy(orderQuery);
        }
    }
}
