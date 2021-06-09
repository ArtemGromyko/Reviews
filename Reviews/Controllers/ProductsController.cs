using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.GET;
using Entities.DataTransferObjects.POST;
using Entities.DataTransferObjects.PUT;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Reviews.ActionFilters;
using Reviews.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.Product.GetAllProductsAsync(false);
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id}", Name = "ProductById")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _repository.Product.GetProductAsync(id, false);
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
        public async Task<IActionResult> GetDirectorsForProduct(Guid id)
        {
            var product = await _repository.Product.GetProductAsync(id, false);
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
        public async Task<IActionResult> GetActorsForProduct(Guid id)
        {
            var product = await _repository.Product.GetProductAsync(id, false);
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

        [HttpGet("collection/{ids}", Name = "ProductCollection")]
        public async Task<IActionResult> GetProductCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null.");
                return BadRequest("Parameter ids is null.");
            }

            var productEntities = await _repository.Product.GetByIdsAsync(ids, false);

            if (ids.Count() != productEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection.");
                return NotFound();
            }

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(productEntities);
            return Ok(productsDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto product)
        {
            if (product == null)
            {
                _logger.LogError("ProductForCreationDto object sent from client is null.");
                return BadRequest("ProductForCreationDto object is null.");
            }

            var productEntity = _mapper.Map<Product>(product);
            _repository.Product.CreateProduct(productEntity);
            await _repository.SaveAsync();

            var productDto = _mapper.Map<ProductDto>(productEntity);
            return CreatedAtRoute("ProductById", new { id = productDto.Id }, productDto);
        }

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProductCollection([FromBody] IEnumerable<ProductForCreationDto> productCollection)
        {
            if (productCollection == null)
            {
                _logger.LogError("Product collection sent from client is null.");
                return BadRequest("Product collection is null.");
            }

            var productEntities = _mapper.Map<IEnumerable<Product>>(productCollection);
            foreach (var p in productEntities)
                _repository.Product.CreateProduct(p);
            await _repository.SaveAsync();

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(productEntities);
            var ids = string.Join(",", productsDto.Select(p => p.Id));

            return CreatedAtRoute("ProductCollection", new { ids }, productsDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _repository.Product.GetProductAsync(id, false);
            if(product == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Product.DeleteProduct(product);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody]ProductForUpdateDto product)
        {
            var productEntity = await _repository.Product.GetProductAsync(id, true);
            if(productEntity == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(product, productEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
