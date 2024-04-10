using EduhubAPI.Models;
using EduhubAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseRepository _courseRepository;
        public CourseController(CourseRepository context)
        {
            _courseRepository = context;
        }
        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var courses = _courseRepository.GetAllCourses();
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
        [HttpPost]
        public IActionResult AddCourse([FromBody] Course course)
        {
            if (course == null)
            {
                return BadRequest();
            }
            var createdCourse = _courseRepository.AddCourse(course);
            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.CourseId }, createdCourse);
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
    }
}
