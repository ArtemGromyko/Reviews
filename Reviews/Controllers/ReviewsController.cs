﻿using AutoMapper;
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

        [HttpGet("collection/{ids}", Name = "ReviewCollection")]
        public IActionResult GetReviewCollection(Guid productId, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null.");
                return BadRequest("Parameter ids is null.");
            }

            var reviewEntities = _repository.Review.GetByIds(productId, ids, false);

            if (ids.Count() != reviewEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection.");
                return NotFound();
            }

            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviewEntities);
            return Ok(reviewsDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
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

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult CreateReviewCollection(Guid productId, [FromBody] IEnumerable<ReviewForCreationDto> reviewCollection)
        {
            if (reviewCollection == null)
            {
                _logger.LogError("Product collection sent from client is null.");
                return BadRequest("Product collection is null.");
            }

            var reviewEntities = _mapper.Map<IEnumerable<Review>>(reviewCollection);
            foreach (var r in reviewEntities)
                _repository.Review.CreateReviewForProduct(productId, r);
            _repository.Save();

            var reviewsDto = _mapper.Map<IEnumerable<ReviewDto>>(reviewEntities);
            var ids = string.Join(",", reviewsDto.Select(p => p.Id));

            return CreatedAtRoute("ReviewCollection", new { productId, ids }, reviewsDto);
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
