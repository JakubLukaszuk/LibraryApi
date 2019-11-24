using LibraryApi.Dtos;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public  class ReviewersController : Controller
    {
        private IReviewerRepository _reviewerRepository;

        public ReviewersController(IReviewerRepository reviewerRepository)
        {
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDataTransferObjectscs>))]
        public IActionResult GetReviewers()
        {
            ICollection<Reviewer> reviewers = _reviewerRepository.GetReviewers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewersDtosList = new List<ReviewerDataTransferObjectscs>();
            foreach (Reviewer reviewer in reviewers)
            {
                reviewersDtosList.Add(new ReviewerDataTransferObjectscs
                {
                    Id = reviewer.Id,
                    FirstName = reviewer.FirstName,
                    LastName = reviewer.LastName
                });

            }

            return Ok(reviewersDtosList);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDataTransferObjectscs>))]
        public IActionResult GetReviewer(int reviewerId)
        {
            Reviewer reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ReviewerDataTransferObjectscs reviewerDots;

                reviewerDots = new ReviewerDataTransferObjectscs
                {
                    Id = reviewer.Id,
                    FirstName = reviewer.FirstName,
                    LastName = reviewer.LastName
                };

       
            return Ok(reviewerDots);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDataTransferObjectscs>))]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            ICollection<Review> reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var revieweDtosList = new List<ReviewDataTransferObjects>();
            foreach (Review reviewer in reviews)
            {
                revieweDtosList.Add(new ReviewDataTransferObjects
                {
                    Id = reviewer.Id,
                    ReviewText = reviewer.ReviewText,
                    Rating = reviewer.Rating,
                    Headline = reviewer.Headline
                });
            }

            return Ok(reviews);
        }

        [HttpGet("reviews/{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDataTransferObjects>))]
        public IActionResult GetReviewerOfReview(int reviewerId)
        {
            Reviewer reviewer = _reviewerRepository.GetRevieverByReview(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ReviewerDataTransferObjectscs reviewerDtos;

            reviewerDtos = new ReviewerDataTransferObjectscs
            {
                Id = reviewer.Id,
                FirstName = reviewer.FirstName,
                LastName = reviewer.LastName
            };
            
            return Ok(reviewer);
        }

    }
}
