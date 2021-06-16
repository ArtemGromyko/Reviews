using Entities.Models;
using Repository.Extensions.Utility;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryProductExtensions
    {
        public static IQueryable<Product> FilterProducts(this IQueryable<Product> products, string categories, string genres, string countries)
        {
            if (!string.IsNullOrWhiteSpace(categories))
            {
                var lowerCaseCategories = categories.Trim().ToLower();
                products = products.Where(p => lowerCaseCategories.Contains(p.Category.ToLower()));
            }

            if (!string.IsNullOrEmpty(genres))
            {
                var lowerCaseGenres = genres.Trim().ToLower();
                products = products.Where(p => lowerCaseGenres.Contains(p.Genre.ToLower()));
            }

            if (!string.IsNullOrEmpty(countries))
            {
                var lowerCaseCountries = countries.Trim().ToLower();
                products = products.Where(p => lowerCaseCountries.Contains(p.Country.ToLower()));
            }

            return products;
        }

        public static IQueryable<Product> Search(this IQueryable<Product> products, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return products;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return products.Where(p => p.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Product> Sort(this IQueryable<Product> products, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return products.OrderBy(p => p.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Product>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return products.OrderBy(p => p.Name);

            return products.OrderBy(orderQuery);
        }
    }
}
