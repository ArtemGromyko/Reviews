

namespace Contracts
{
    public interface IRepositoryManager
    {
        IProductRepository Product { get; }
        IPersonRepository Person { get; }
        IReviewRepository Review { get; }
        void Save();
    }
}
