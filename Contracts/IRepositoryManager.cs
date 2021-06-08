using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IProductRepository Product { get; }
        IPersonRepository Person { get; }
        IReviewRepository Review { get; }
        Task SaveAsync();
    }
}
