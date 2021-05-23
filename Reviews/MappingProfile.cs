using AutoMapper;
using Entities.DataTransferObjects.GET;
using Entities.Models;

namespace Reviews
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>();
        }
    }
}
