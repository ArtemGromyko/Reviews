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
    [Route("api/products")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ProductDto> _dataShaper;

        public ProductsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<ProductDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetProducts([FromQuery]ProductParameters productParameters)
        {
            var products = await _repository.Product.GetAllProductsAsync(productParameters, false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(products.MetaData));

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(_dataShaper.ShapeData(productsDto, productParameters.Fields));
        }

        [HttpGet("{id}", Name = "ProductById")]
        [ServiceFilter(typeof(ValidationProductExistsAttribute))]
        public IActionResult GetProduct(Guid id)
        {
            var product = HttpContext.Items["product"] as Product;
            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);
        }

        [HttpGet("{id}/directors")]
        [ServiceFilter(typeof(ValidationProductExistsAttribute))]
        public IActionResult GetDirectorsForProduct(Guid id)
        {
            var product = HttpContext.Items["product"] as Product;
            var directors = _mapper.Map<IEnumerable<PersonDto>>(product.Directors);

            return Ok(directors);
          
        }

        [HttpGet("{id}/actors")]
        [ServiceFilter(typeof(ValidationProductExistsAttribute))]
        public IActionResult GetActorsForProduct(Guid id)
        {
            var product = HttpContext.Items["product"] as Product;

            var actors = _mapper.Map<IEnumerable<PersonDto>>(product.Actors);
            return Ok(actors);
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

        [HttpPost, Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            _repository.Product.CreateProduct(productEntity);
            await _repository.SaveAsync();

            var productDto = _mapper.Map<ProductDto>(productEntity);
            return CreatedAtRoute("ProductById", new { id = productDto.Id }, productDto);
        }

        [HttpPost("collection"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProductCollection([FromBody] IEnumerable<ProductForCreationDto> productCollection)
        {
            var productEntities = _mapper.Map<IEnumerable<Product>>(productCollection);
            foreach (var p in productEntities)
                _repository.Product.CreateProduct(p);
            await _repository.SaveAsync();

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(productEntities);
            var ids = string.Join(",", productsDto.Select(p => p.Id));

            return CreatedAtRoute("ProductCollection", new { ids }, productsDto);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationProductExistsAttribute))]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = HttpContext.Items["product"] as Product;

            _repository.Product.DeleteProduct(product);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidationProductExistsAttribute))]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody]ProductForUpdateDto product)
        {
            var productEntity = HttpContext.Items["product"] as Product;

            _mapper.Map(product, productEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{id}"), Authorize(Roles = "Administrator")]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationProductExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateProduct(Guid id, [FromBody]JsonPatchDocument<ProductForUpdateDto> patchDoc)
        {
            var productEntity = HttpContext.Items["product"] as Product;

            var productToPatch = _mapper.Map<ProductForUpdateDto>(productEntity);
            patchDoc.ApplyTo(productToPatch, ModelState);
            TryValidateModel(productToPatch);
            if(!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(productToPatch, productEntity);

            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetProductsOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, OPTIONS, HEAD");

            return Ok();
        }
    }
}
