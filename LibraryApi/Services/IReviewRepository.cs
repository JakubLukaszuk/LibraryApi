using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace LibraryApi.Services
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfBook(int bookId);
        Book GetBookOfAReview(int reviewId);
        bool IsReviewExist(int reviewId);
        bool DeleteReview(Review reviewToDelete);
        bool CreateReview(Review reviewToCreate);
        bool UpdateReview(Review reviewToUpdate);
        bool DeleteReviews(List<Review> list);
    }
}
