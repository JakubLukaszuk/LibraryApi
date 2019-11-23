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
        ICollection<Review> GetReviewsOfReviewer(int reviewerId);
        ICollection<Review> GetReviewsOfBook(int bookId);
        bool IsReviewExist(int reviewId);
    }
}
