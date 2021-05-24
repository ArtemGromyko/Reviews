using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public IEnumerable<Person> GetAllPersons(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(p => p.Name)
            .ToList();

        public Person GetPerson(Guid personId, bool trackChanges) =>
            FindByCondition(p => p.Id.Equals(personId), trackChanges).SingleOrDefault();

        public void CreatePerson(Person person) => Create(person);
    }
}
