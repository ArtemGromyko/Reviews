using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.GET;
using Entities.DataTransferObjects.POST;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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

        [HttpPost]
        public IActionResult CreatePerson([FromBody]PersonForCreationDto person)
        {
            if(person == null)
            {
                _logger.LogError($"PersonForCreationDto object sent from client is null");
                return BadRequest("PersonForCreationDto object is null.");
            }

            var personEntity = _mapper.Map<Person>(person);

            _repository.Person.CreatePerson(personEntity);
            _repository.Save();

            var personToReturn = _mapper.Map<PersonDto>(personEntity);

            return CreatedAtRoute("PersonById", new { id = personToReturn.Id}, personToReturn);
        }
    }
}
