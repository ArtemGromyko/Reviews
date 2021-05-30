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
        public IActionResult GetReviewsForProduct(Guid productId)
        {
            var product = _repository.Product.GetProduct(productId, false);
            if(product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }

            var reviews = _repository.Review.GetReviews(productId, false);
            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

            return Ok(reviewsDto);
        }

        [HttpGet("{id}", Name = "GetReviewForProduct")]
        public IActionResult GetReviewForProduct(Guid productId, Guid id)
        {
            var product = _repository.Product.GetProduct(productId, false);
            if(product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }

            var review = _repository.Review.GetReview(productId, id, false);
            if(review == null)
            {
                _logger.LogInfo($"Review with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var reviewDto = _mapper.Map<ReviewDto>(review);
            return Ok(reviewDto);
        }

        [HttpPost]
        public IActionResult CreateReviewForProduct(Guid productId, [FromBody] ReviewForCreationDto review)
        {
            if (review == null)
            {
                _logger.LogError("ReviewForCreationDto object sent from client is null.");
                return BadRequest("ReviewForCreationDto object is null.");
            }

            var product = _repository.Product.GetProduct(productId, false);
            if (product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }

            var reviewEntity = _mapper.Map<Review>(review);

            _repository.Review.CreateReviewForProduct(productId, reviewEntity);
            _repository.Save();

            var reviewDto = _mapper.Map<ReviewDto>(reviewEntity);

            return CreatedAtRoute("GetReviewForProduct", new { productId, id = reviewDto.Id }, reviewDto);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReviewForProduct(Guid productId, Guid id)
        {
            var product = _repository.Product.GetProduct(productId, false);
            if(product == null)
            {
                _logger.LogInfo($"Product with id: {productId} doesn't exist in the database.");
                return NotFound();
            }

            var reviewForProduct = _repository.Review.GetReview(productId, id, false);
            if(reviewForProduct == null)
            {
                _logger.LogInfo($"Review with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Review.DeleteReview(reviewForProduct);
            _repository.Save();

            return NoContent();
        }

    }
}
