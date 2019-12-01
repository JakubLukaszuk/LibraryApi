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
    public class ReviewsController : Controller
    {
        private IReviewRepository _reviewsRepository;
        private IBookRepository _bookRepository;
        private IReviewerRepository _reviewerRepository;

        public ReviewsController(IReviewRepository reviewsRepository, IBookRepository bookRepository, IReviewerRepository reviewerRepository)
        {
            _reviewsRepository = reviewsRepository;
            _bookRepository = bookRepository;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDataTransferObjects>))]
        public IActionResult GetReviews()
        {
            ICollection<Review> reviews = _reviewsRepository.GetReviews();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewsDtosList = new List<ReviewDataTransferObjects>();
            foreach (Review review in reviews)
            {
                reviewsDtosList.Add(new ReviewDataTransferObjects
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });

            }

            return Ok(reviewsDtosList);
        }

        [HttpGet("{reviewId}", Name = "GetReview")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDataTransferObjects>))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewsRepository.IsReviewExist(reviewId))
                NotFound();

            Review review = _reviewsRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ReviewDataTransferObjects reviewDtos;

                reviewDtos = new ReviewDataTransferObjects
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                };

            return Ok(reviewDtos);
        }

        [HttpGet("book/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDataTransferObjects>))]
        public IActionResult GetReviewsOfBook(int bookId)
        {
            if (!_bookRepository.IsBookExist(bookId))
            {
                return NotFound();
            }

            ICollection<Review> reviews = _reviewsRepository.GetReviewsOfBook(bookId);

            var reviewsDtosList = new List<ReviewDataTransferObjects>();
            foreach (Review review in reviews)
            {
                reviewsDtosList.Add(new ReviewDataTransferObjects
                {
                    Id = review.Id,
                    Headline = review.Headline,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });

            }
            return Ok(reviewsDtosList);
        }

        [HttpGet("{reviewId}/book")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDataTransferObjects>))]
        public IActionResult GetBookOfReview(int reviewId)
        {
            Book book = _reviewsRepository.GetBookOfAReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            BookDataTransferObjects bookDtos;

            bookDtos = new BookDataTransferObjects
            {
                Id = book.Id,
                Title = book.Title,
                Ibsn = book.Isbn,
                DatePublished = book.DatePublished
            };

            return Ok(bookDtos);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Review))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateReview([FromBody] Review reviewToCreate)
        {
            if (reviewToCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.IsReviewerExist(reviewToCreate.Reviewer.Id))
            {
                ModelState.AddModelError("", "Reviewer dosen't exist!");
            }

            if (!_bookRepository.IsBookExist(reviewToCreate.Book.Id))
            {
                ModelState.AddModelError("", "Book dosen't exist!");
            }

            reviewToCreate.Book = _bookRepository.GetBook(reviewToCreate.Book.Id);
            reviewToCreate.Reviewer = _reviewerRepository.GetReviewer(reviewToCreate.Reviewer.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewsRepository.CreateReview(reviewToCreate))
            {
                ModelState.AddModelError("", "Somethink went wrong when saing review! Review has not been saved");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCountry", new { reviewId = reviewToCreate.Id }, reviewToCreate);
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReview(int reviewId, [FromBody] Review reviewToUpdate)
        {
            if (reviewToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.IsReviewerExist(reviewId))
            {
                ModelState.AddModelError("", "Review dosen't exist!");
            }

            if (reviewId !=  reviewToUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.IsReviewerExist(reviewToUpdate.Reviewer.Id))
            {
                ModelState.AddModelError("", "Reviewer dosen't exist!");
            }

            if (!_bookRepository.IsBookExist(reviewToUpdate.Book.Id))
            {
                ModelState.AddModelError("", "Book dosen't exist!");
            }

            reviewToUpdate.Book = _bookRepository.GetBook(reviewToUpdate.Book.Id);
            reviewToUpdate.Reviewer = _reviewerRepository.GetReviewer(reviewToUpdate.Reviewer.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewsRepository.UpdateReview(reviewToUpdate))
            {
                ModelState.AddModelError("", "Somethink went wrong when updating review! Review has not been saved");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewsRepository.IsReviewExist(reviewId))
                return NotFound();

            var reviewToDelete = _reviewsRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewsRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Somethink went wrong when deleting review! Review has not been deleted");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
