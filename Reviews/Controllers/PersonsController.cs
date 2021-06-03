using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.GET;
using Entities.DataTransferObjects.POST;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Reviews.ActionFilters;
using Reviews.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reviews.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public PersonsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPersons()
        {
            var persons = _repository.Person.GetAllPersons(trackChanges: false);
            var personsDto = _mapper.Map<IEnumerable<PersonDto>>(persons);

            return Ok(personsDto);
        }

        [HttpGet("{id}", Name = "PersonById")]
        public IActionResult GetPerson(Guid id)
        {
            var person = _repository.Person.GetPerson(id, false);

            if(person == null)
            {
                _logger.LogInfo($"Person with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var personDto = _mapper.Map<PersonDto>(person);
                return Ok(personDto);
            }
        }

        [HttpGet("collection/{ids}", Name = "PersonCollection")]
        public IActionResult GetPersonCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null.");
                return BadRequest("Parameter ids is null.");
            }

            var personEntities = _repository.Person.GetByIds(ids, false);

            if (ids.Count() != personEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection.");
                return NotFound();
            }

            var personsDto = _mapper.Map<IEnumerable<PersonDto>>(personEntities);
            return Ok(personsDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult CreatePerson([FromBody]PersonForCreationDto person)
        {
            var personEntity = _mapper.Map<Person>(person);

            _repository.Person.CreatePerson(personEntity);
            _repository.Save();

            var personToReturn = _mapper.Map<PersonDto>(personEntity);

            return CreatedAtRoute("PersonById", new { id = personToReturn.Id}, personToReturn);
        }

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult CreatePersonCollection([FromBody] IEnumerable<PersonForCreationDto> personCollection)
        {
            var personEntities = _mapper.Map<IEnumerable<Person>>(personCollection);
            foreach (var p in personEntities)
                _repository.Person.CreatePerson(p);
            _repository.Save();

            var personsDto = _mapper.Map<IEnumerable<PersonDto>>(personEntities);
            var ids = string.Join(",", personsDto.Select(p => p.Id));

            return CreatedAtRoute("PersonCollection", new { ids }, personsDto);
        }
    }
}
