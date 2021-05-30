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
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ProductsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _repository.Product.GetAllProducts(false);
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id}", Name = "ProductById")]
        public IActionResult GetProduct(Guid id)
        {
            var product = _repository.Product.GetProduct(id, false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var productDto = _mapper.Map<ProductDto>(product);
                return Ok(productDto);
            }
        }

        [HttpGet("{id}/directors")]
        public IActionResult GetDirectorsForProduct(Guid id)
        {
            var product = _repository.Product.GetProduct(id, false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var directors = _mapper.Map<IEnumerable<PersonDto>>(product.Directors);
                return Ok(directors);
            }
        }

        [HttpGet("{id}/actors")]
        public IActionResult GetActorsForProduct(Guid id)
        {
            var product = _repository.Product.GetProduct(id, false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var actors = _mapper.Map<IEnumerable<PersonDto>>(product.Actors);
                return Ok(actors);
            }
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductForCreationDto product)
        {
            if (product == null)
            {
                _logger.LogError("ProductForCreationDto object sent from client is null.");
                return BadRequest("ProductForCreationDto object is null.");
            }

            var productEntity = _mapper.Map<Product>(product);
            _repository.Product.CreateProduct(productEntity);
            _repository.Save();
             
            var productDto = _mapper.Map<ProductDto>(productEntity);
            return CreatedAtRoute("ProductById", new { id = productDto.Id}, productDto); 
        }
    }
}
