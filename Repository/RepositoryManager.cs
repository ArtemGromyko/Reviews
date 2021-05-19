using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private IPersonRepository _personRepository;
        private IProductRepository _productRepository;
        private IReviewRepository _reviewRepository;
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public IProductRepository Product
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(_repositoryContext);

                return _productRepository;
            }
        }

        public IPersonRepository Person
        {
            get
            {
                if (_personRepository == null)
                    _personRepository = new PersonRepository(_repositoryContext);

                return _personRepository;
            }
        }

        public IReviewRepository Review
        {
            get
            {
                if (_reviewRepository == null)
                    _reviewRepository = new ReviewRepository(_repositoryContext);

                return _reviewRepository;
            }
        }

        public void Save() => _repositoryContext.SaveChanges();
    }
}
