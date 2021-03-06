using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Parameters;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public async Task<PagedList<Person>> GetAllPersonsAsync(PersonParameters personParameters, bool trackChanges)
        {
            var persons = await FindAll(trackChanges)
                .FilterPersons(personParameters.MinBirthDate, personParameters.MaxBirthDate, personParameters.BirthPlace,
                personParameters.MinHeight, personParameters.MaxHeight)
                .Search(personParameters.SearchTerm)
                .Sort(personParameters.OrderBy)
                .ToListAsync();

            return PagedList<Person>.ToPagedList(persons, personParameters.PageNumber, personParameters.PageSize);
        }

        public async Task<Person> GetPersonAsync(Guid personId, bool trackChanges) =>
            await FindByCondition(p => p.Id.Equals(personId), trackChanges).SingleOrDefaultAsync();

        public void CreatePerson(Person person) => Create(person);

        public async Task<IEnumerable<Person>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

        public void DeletePerson(Person person) => Delete(person);
    }
}
