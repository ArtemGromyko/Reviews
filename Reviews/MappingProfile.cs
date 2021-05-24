using AutoMapper;
using Entities.DataTransferObjects.GET;
using Entities.DataTransferObjects.POST;
using Entities.Models;
using System.Collections.Generic;
using System.Text;

namespace Reviews
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>();

            CreateMap<Product, ProductDto>()
                .ForMember(pd => pd.Actors, opt => opt.MapFrom(x => GetPersons(x.Actors)))
                .ForMember(pd => pd.Directors, opt => opt.MapFrom(x => GetPersons(x.Directors)));

            CreateMap<Review, ReviewDto>();

            CreateMap<PersonForCreationDto, Person>();
        }

        private string GetPersons(IEnumerable<Person> people)
        {
            var sb = new StringBuilder("");

            foreach (var person in people)
                sb.Append(person.Name + ", ");

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }
    }
}
