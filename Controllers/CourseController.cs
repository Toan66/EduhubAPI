﻿using EduhubAPI.Helpers;
using EduhubAPI.Dtos;
using EduhubAPI.Models;
using EduhubAPI.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseRepository _courseRepository;
        private readonly JwtService _jwtService;
        public CourseController(CourseRepository context, JwtService jwtService)
        {
            _courseRepository = context;
            _jwtService = jwtService;
        }
        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var courses = _courseRepository.GetAllCourses().Where(x => x.ApprovalStatus == true);
            return Ok(courses);
        }
        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            var course = _courseRepository.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }
        [HttpPost("create")]
        public IActionResult AddCourse(AddCourseDto addCourse)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized();
                }
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);

                var course = new Course
                {
                    TeacherId = userId,
                    CourseName = addCourse.CourseName,
                    CourseDescription = addCourse.CourseDescription,
                    ApprovalStatus = false,
                    CategoryId = addCourse.CategoryId,
                    FeatureImage = addCourse.FeatureImage
                };

                return Ok(_courseRepository.AddCourse(course));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] Course course)
        {
            if (course == null || course.CourseId != id)
            {
                return BadRequest();
            }
            var existingCourse = _courseRepository.GetCourseById(id);
            if (existingCourse == null)
            {
                return NotFound();
            }
            _courseRepository.UpdateCourse(course);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _courseRepository.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            _courseRepository.DeleteCourse(id);
            return NoContent();
        }
        [HttpGet("{id}/details")]
        public IActionResult GetCourseDetails(int id)
        {
            var courseDetails = _courseRepository.GetCourseDetails(id);
            if (courseDetails == null)
            {
                return NotFound("Không tìm thấy khóa học.");
            }
        
            return Ok(courseDetails);
        }

        [HttpGet("ByTeacher")]
        public IActionResult GetCoursesByTeacher()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized("Không có token được cung cấp.");
                }
        
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
        
                var courses = _courseRepository.GetCoursesByTeacherId(userId);
                if (courses == null || !courses.Any())
                {
                    return NotFound("Không tìm thấy khóa học nào.");
                }
        
                return Ok(courses);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
