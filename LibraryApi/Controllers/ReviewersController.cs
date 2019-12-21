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
        private IReviewRepository reviewRepository;

        public ReviewersController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
            this._reviewerRepository = reviewerRepository;
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

        [HttpGet("{reviewerId}", Name = "GetReviewer") ]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDataTransferObjectscs>))]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.IsReviewerExist(reviewerId))
                return NotFound();

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

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateReviewer([FromBody] Reviewer reviewerToCreate)
        {
            if (reviewerToCreate == null)
            {
                return BadRequest(ModelState);
            }

            var country = _reviewerRepository.GetReviewers().Where(r =>
                r.FirstName.Trim().ToUpper() == reviewerToCreate.FirstName.Trim().ToUpper()&&
                r.LastName.Trim().ToUpper() == reviewerToCreate.LastName.Trim().ToUpper()).FirstOrDefault();
                

            if (country != null)
            {
                ModelState.AddModelError("", $"Reviewer: {reviewerToCreate.FirstName}   {reviewerToCreate.LastName} already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.CreateReviewer(reviewerToCreate))
            {
                ModelState.AddModelError("", $"Somethink went wrong. {reviewerToCreate.FirstName} {reviewerToCreate.LastName} not saved");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReviewer", new { reviewerId = reviewerToCreate.Id }, reviewerToCreate);
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.IsReviewerExist(reviewerId))
            {
                return BadRequest(ModelState);
            }

            var reviewer = _reviewerRepository.GetReviewer(reviewerId);
            var reviews = _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviewer))
            {
                ModelState.AddModelError("", $"Somethink went wrong. {reviewer.FirstName} {reviewer.LastName} not deleted");
                return StatusCode(500, ModelState);
            }

            if (!reviewRepository.DeleteReviews(reviews.ToList()))
            {
                ModelState.AddModelError("", $"Somethink went wrong. {reviewer.FirstName} {reviewer.LastName} reviews not deleted");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] Reviewer updatedReviewerInfno)
        {
            if (updatedReviewerInfno == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewerId != updatedReviewerInfno.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.IsReviewerExist(reviewerId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.UpdaeReviewer(updatedReviewerInfno))
            {
                ModelState.AddModelError("", $"Somethink went wrong when updating: {updatedReviewerInfno.FirstName} {updatedReviewerInfno.LastName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
