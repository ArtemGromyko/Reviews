using AutoMapper;
using Contracts;
using Entities.DataTransferObjects.GET;
using Entities.DataTransferObjects.POST;
using Entities.DataTransferObjects.PUT;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Reviews.ActionFilters;
using Reviews.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reviews.Controllers
{
    [Route("api/products/{productId}/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ReviewsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidationProductExistsAttribute))]
        public async Task<IActionResult> GetReviewsForProduct(Guid productId)
        {
            var reviews = await _repository.Review.GetReviewsAsync(productId, false);
            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return Ok(reviewsDto);
        }

        [HttpGet("{id}", Name = "GetReviewForProduct")]
        [ServiceFilter(typeof(ValidationReviewForProductExistsAttribute))]
        public IActionResult GetReviewForProduct(Guid productId, Guid id)
        {
            var review = HttpContext.Items["review"] as Review;
            var reviewDto = _mapper.Map<ReviewDto>(review);

            return Ok(reviewDto);
        }

        [HttpGet("collection/{ids}", Name = "ReviewCollection")]
        public async Task<IActionResult> GetReviewCollection(Guid productId, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null.");
                return BadRequest("Parameter ids is null.");
            }

            var reviewEntities = await _repository.Review.GetByIdsAsync(productId, ids, false);

            if (ids.Count() != reviewEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection.");
                return NotFound();
            }

            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviewEntities);
            return Ok(reviewsDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidationProductExistsAttribute))]
        public async Task<IActionResult> CreateReviewForProduct(Guid productId, [FromBody] ReviewForCreationDto review)
        {
            var reviewEntity = _mapper.Map<Review>(review);

            _repository.Review.CreateReviewForProduct(productId, reviewEntity);
            await _repository.SaveAsync();

            var reviewDto = _mapper.Map<ReviewDto>(reviewEntity);

            return CreatedAtRoute("GetReviewForProduct", new { productId, id = reviewDto.Id }, reviewDto);
        }

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidationProductExistsAttribute))]
        public async Task<IActionResult> CreateReviewCollection(Guid productId, [FromBody] IEnumerable<ReviewForCreationDto> reviewCollection)
        {
            var reviewEntities = _mapper.Map<IEnumerable<Review>>(reviewCollection);
            foreach (var r in reviewEntities)
                _repository.Review.CreateReviewForProduct(productId, r);
            await _repository.SaveAsync();

            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviewEntities);
            var ids = string.Join(",", reviewsDto.Select(p => p.Id));

            return CreatedAtRoute("ReviewCollection", new { productId, ids }, reviewsDto);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidationReviewForProductExistsAttribute))]
        public async Task<IActionResult> DeleteReviewForProduct(Guid productId, Guid id)
        {
            var reviewForProduct = HttpContext.Items["review"] as Review;

            _repository.Review.DeleteReview(reviewForProduct);
            await _repository.SaveAsync();

            return NoContent();
        }


        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationNullArgumentAttribute))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateReviewForProduct(Guid productId, Guid id, [FromBody]ReviewForUpdateDto review)
        {
            var reviewEntity = HttpContext.Items["entity"] as Review;

            _mapper.Map(review, reviewEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        
    }
}
