using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.GET;
using Entities.DataTransferObjects.POST;
using Entities.DataTransferObjects.PUT;
using Entities.Models;
using Entities.RequestFeatures.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reviews.ActionFilters;
using Reviews.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reviews.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/persons")]
    [ApiController]
    [Authorize]
    public class PersonsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<PersonDto> _dataShaper;

        public PersonsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<PersonDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetPersons([FromQuery]PersonParameters personParameters)
        {
            if (!personParameters.ValidParametersRange)
                return BadRequest("Max birth date can't be lower than min birth date and max height can't be lower than min height.");

            var persons = await _repository.Person.GetAllPersonsAsync(personParameters, false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(persons.MetaData));

            var personsDto = _mapper.Map<IEnumerable<PersonDto>>(persons);

            return Ok(_dataShaper.ShapeData(personsDto, personParameters.Fields));
        }

        [HttpGet("{id}", Name = "PersonById")]
        [ServiceFilter(typeof(ValidationPersonExistsAttribute))]
        public IActionResult GetPerson(Guid id)
        {
            var person = HttpContext.Items["person"] as Person;

            var personDto = _mapper.Map<PersonDto>(person);
            return Ok(personDto);
        }

        [HttpGet("collection/{ids}", Name = "PersonCollection")]
        public async Task<IActionResult> GetPersonCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null.");
                return BadRequest("Parameter ids is null.");
            }

            var personEntities = await _repository.Person.GetByIdsAsync(ids, false);

            if (ids.Count() != personEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection.");
                return NotFound();
            }

            var personsDto = _mapper.Map<IEnumerable<PersonDto>>(personEntities);
            return Ok(personsDto);
        }

        [HttpPost, Authorize(Roles ="Administrator")]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePerson([FromBody]PersonForCreationDto person)
        {
            var personEntity = _mapper.Map<Person>(person);

            _repository.Person.CreatePerson(personEntity);
            await _repository.SaveAsync();

            var personToReturn = _mapper.Map<PersonDto>(personEntity);

            return CreatedAtRoute("PersonById", new { id = personToReturn.Id}, personToReturn);
        }

        [HttpPost("collection"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePersonCollection([FromBody] IEnumerable<PersonForCreationDto> personCollection)
        {
            var personEntities = _mapper.Map<IEnumerable<Person>>(personCollection);
            foreach (var p in personEntities)
                _repository.Person.CreatePerson(p);
            await _repository.SaveAsync();

            var personsDto = _mapper.Map<IEnumerable<PersonDto>>(personEntities);
            var ids = string.Join(",", personsDto.Select(p => p.Id));

            return CreatedAtRoute("PersonCollection", new { ids }, personsDto);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationPersonExistsAttribute))]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var person = HttpContext.Items["person"] as Person;

            _repository.Person.DeletePerson(person);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidationPersonExistsAttribute))]
        public async Task<IActionResult> UpdatePerson(Guid id, [FromBody]PersonForUpdateDto person)
        {
            var personEntity = HttpContext.Items["person"] as Person;

            _mapper.Map(person, personEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{id}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationPersonExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdatePerson(Guid id, [FromBody] JsonPatchDocument<PersonForUpdateDto> patchDoc)
        {
            var personEntity = HttpContext.Items["person"] as Person;

            var personToPatch = _mapper.Map<PersonForUpdateDto>(personEntity);
            patchDoc.ApplyTo(personToPatch, ModelState);
            TryValidateModel(personToPatch);
            if(!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(personToPatch, personEntity);

            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetPersonsOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, OPTIONS, HEAD");

            return Ok();
        }
    }
}
