using Entities.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Contracts
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetAllPersons(bool trackChanges);
        Person GetPerson(Guid personId, bool trackChanges);
    }
}
