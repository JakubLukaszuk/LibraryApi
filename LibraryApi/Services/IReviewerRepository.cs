using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Models;

namespace LibraryApi.Services
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        Reviewer GetRevieverByReview(int reviewId);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool IsReviewerExist (int reviewerId);
    }
}
