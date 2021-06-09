using AutoMapper;
using Entities.DataTransferObjects.GET;
using Entities.DataTransferObjects.POST;
using Entities.DataTransferObjects.PUT;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reviews
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>();

            CreateMap<Product, ProductDto>()
                .ForMember(pd => pd.Actors, opt => opt.MapFrom(x => GetNames(x.Actors)))
                .ForMember(pd => pd.Directors, opt => opt.MapFrom(x => GetNames(x.Directors)));

            CreateMap<Review, ReviewDto>();

            CreateMap<PersonForCreationDto, Person>();
            CreateMap<ProductForCreationDto, Product>();
            CreateMap<ReviewForCreationDto, Review>();

            CreateMap<PersonForUpdateDto, Person>();
            CreateMap<ProductForUpdateDto, Product>();
            CreateMap<ReviewForUpdateDto, Review>();
        }

        private string GetNames(IEnumerable<Person> people)
        {   
            if(people != null && people.Count() != 0)
            {
                var sb = new StringBuilder("");

                foreach (var person in people)
                    sb.Append(person.Name + ", ");

                sb.Remove(sb.Length - 2, 2);

                return sb.ToString();
            }

            return "empty";
        }
    }
}
