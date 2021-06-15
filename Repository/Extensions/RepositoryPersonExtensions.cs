using Entities.Models;
using System;
using System.Linq;

namespace Repository.Extensions
{
    public static class RepositoryPersonExtensions
    {
        public static IQueryable<Person> FilterPersons(this IQueryable<Person> persons, DateTime minBirthDate, DateTime maxBirthDate, 
        string birthPlace, double minHeight, double maxHeight)
        {
            if(!string.IsNullOrWhiteSpace(birthPlace))
            {
                var lowerCaseBirthPlace = birthPlace.Trim().ToLower();
                persons = persons.Where(p => birthPlace.ToLower().Contains(p.BirthPlace));
            }

            return persons.Where(p => (p.BirthDate >= minBirthDate && p.BirthDate <= maxBirthDate) &&
                (p.Height >= minHeight && p.Height <= maxHeight));
        }

        public static IQueryable<Person> Search(this IQueryable<Person> persons, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return persons;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return persons.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }
    }
}
