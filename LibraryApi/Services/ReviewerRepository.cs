using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApi.Models;

namespace LibraryApi.Services
{
    public class ReviewerRepository : IReviewerRepository
    {
        private LibraryDbContext _reviewerContext;

        public ReviewerRepository(LibraryDbContext reviewerContext)
        {
            _reviewerContext = reviewerContext;
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _reviewerContext.Reviewers.ToList();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public Reviewer GetRevieverByReview(int reviewId)
        {
            return _reviewerContext.Reviews.Where(r => r.Id == reviewId).Select(rr => rr.Reviewer).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _reviewerContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool IsReviewerExist(int reviewerId)
        {
            return _reviewerContext.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _reviewerContext.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _reviewerContext.Remove(reviewer);
            return Save();
        }

        public bool UpdaeReviewer(Reviewer reviewer)
        {
            _reviewerContext.Update(reviewer);
            return Save();
        }

        public bool Save()
        {
            return _reviewContext.SaveChanges() >= 0 ? true : false;
        }
    }
}
