using EduhubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduhubAPI.Repositories
{
    public class ReviewRepository
    {
        private readonly EDUHUBContext _context;

        public ReviewRepository(EDUHUBContext context)
        {
            _context = context;
        }

        // Add a new review
        public Review AddReview(Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return review;
        }

        // Get a review by ID
        public Review GetReviewById(int reviewId)
        {
            return _context.Reviews.FirstOrDefault(r => r.ReviewId == reviewId);
        }

        // Get all reviews for a course
        public IEnumerable<Review> GetReviewsByCourseId(int courseId)
        {
            return _context.Reviews.Where(r => r.CourseId == courseId).ToList();
        }

        // Update a review
        public void UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            _context.SaveChanges();
        }

        // Delete a review
        public void DeleteReview(int reviewId)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.ReviewId == reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Review with ID: {reviewId} does not exist.");
            }
        }
        public Review GetReviewByUserIdAndCourseId(int userId, int courseId)
        {
            return _context.Reviews.FirstOrDefault(r => r.UserId == userId && r.CourseId == courseId);
        }

    }
}