using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace LibraryApi.Services
{
    interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfBook(int bookId);
        Book GetBookOfAReview(int reviewId);
        bool IsReviewExist(int reviewId);
    }
}
