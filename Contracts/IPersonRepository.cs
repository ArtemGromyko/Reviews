using Entities.Models;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPersonRepository
    {
        Task<PagedList<Person>> GetAllPersonsAsync(PersonParameters personParameters, bool trackChanges);
        Task<Person> GetPersonAsync(Guid personId, bool trackChanges);
        Task<IEnumerable<Person>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void CreatePerson(Person person);
        void DeletePerson(Person person);
    }
}
