using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(p => p.Name)
            .ToListAsync();

        public async Task<Person> GetPersonAsync(Guid personId, bool trackChanges) =>
            await FindByCondition(p => p.Id.Equals(personId), trackChanges).SingleOrDefaultAsync();

        public void CreatePerson(Person person) => Create(person);

        public async Task<IEnumerable<Person>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
    }
}
