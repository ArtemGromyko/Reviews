﻿using Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reviews.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public PersonsController(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetPersons()
        {
            try
            {
                var persons = _repository.Person.GetAllPersons(trackChanges: false);

                return Ok(persons);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetPersons)} action {ex}");

                return StatusCode(500, "Internal server error.");
            }
        }
    }
}