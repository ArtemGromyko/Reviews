using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllPersonsAsync(bool trackChanges);
        Task<Person> GetPersonAsync(Guid personId, bool trackChanges);
        Task<IEnumerable<Person>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void CreatePerson(Person person);
    }
}
