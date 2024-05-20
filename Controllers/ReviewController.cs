using EduhubAPI.Dtos;
using EduhubAPI.Helpers;
using EduhubAPI.Models;
using EduhubAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewRepository _reviewRepository;
        private readonly JwtService _jwtService;

        public ReviewController(ReviewRepository reviewRepository, JwtService jwtService)
        {
            _reviewRepository = reviewRepository;
            _jwtService = jwtService;
        }

        [HttpPost("{courseId}")]
        public IActionResult AddReview(int courseId, [FromBody] AddReviewDto reviewDto)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized("No JWT token provided.");
                }

                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);

                var review = new Review
                {
                    CourseId = courseId,
                    UserId = userId,
                    Rating = reviewDto.Rating,
                    Comment = reviewDto.Comment,
                    ReviewDate = DateTime.Now
                };

                _reviewRepository.AddReview(review);

                return Ok("Review added successfully.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{courseId}/canReview")]
        public IActionResult CanUserReview(int courseId)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized("No JWT token provided.");
                }

                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer); // Assuming the userId is stored in the Issuer field

                // Check if the user has already submitted a review for the course
                var existingReview = _reviewRepository.GetReviewByUserIdAndCourseId(userId, courseId);
                if (existingReview != null)
                {
                    return Ok(new { CanReview = false, Message = "You have already reviewed this course." });
                }
                else
                {
                    return Ok(new { CanReview = true, Message = "You can review this course." });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{courseId}/updateReview")]
        public IActionResult UpdateReview(int courseId, [FromBody] AddReviewDto reviewDto)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized("No JWT token provided.");
                }

                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);

                var existingReview = _reviewRepository.GetReviewByUserIdAndCourseId(userId, courseId);
                if (existingReview == null)
                {
                    return NotFound("Review not found.");
                }

                existingReview.Rating = reviewDto.Rating;
                existingReview.Comment = reviewDto.Comment;
                existingReview.ReviewDate = DateTime.Now;

                _reviewRepository.UpdateReview(existingReview);

                return Ok("Review updated successfully.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}