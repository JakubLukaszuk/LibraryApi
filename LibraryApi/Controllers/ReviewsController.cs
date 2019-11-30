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

        public ReviewsController(IReviewRepository reviewsRepository, IBookRepository bookRepository)
        {
            _reviewsRepository = reviewsRepository;
            _bookRepository = bookRepository;
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

        [HttpGet("{reviewId}")]
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
    }
}
