using Contracts;
using Entities.RequestFeatures.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Reviews.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/persons")]
    [ApiController]
    public class PersonsV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;

        public PersonsV2Controller(IRepositoryManager repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPersons([FromQuery] PersonParameters personParameters)
        {
            var persons = await _repository.Person.GetAllPersonsAsync(personParameters ,false);

            return Ok(persons);
        }
    }
}
