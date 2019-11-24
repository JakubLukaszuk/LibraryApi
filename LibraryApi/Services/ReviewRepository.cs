using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Models;

namespace LibraryApi.Services
{
    public class ReviewRepository : IReviewRepository
    {
        LibraryDbContext _reviewContext;

        public ReviewRepository(LibraryDbContext reviewContext)
        {
            _reviewContext = reviewContext;
        }

        public Book GetBookOfAReview(int reviewId)
        {
            return _reviewContext.Reviews.Where(r => r.Id == reviewId).Select(b => b.Book).FirstOrDefault();
        }

        public Review GetReview(int reviewId)
        {
            return _reviewContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _reviewContext.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfBook(int bookId)
        {
            return _reviewContext.Reviews.Where(r => r.Book.Id == bookId).ToList();
        }

        public bool IsReviewExist(int reviewId)
        {
            return _reviewContext.Reviews.Any(r => r.Id == reviewId);
        }
    }
}
