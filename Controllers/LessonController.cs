using EduhubAPI.Dtos;
using EduhubAPI.Helpers;
using EduhubAPI.Models;
using EduhubAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly LessonRepository _lessonRepository;
        private readonly JwtService _jwtService;
        public LessonController(LessonRepository lessonRepository, JwtService jwtService)
        {
            _lessonRepository = lessonRepository;
            _jwtService = jwtService;
        }

        // Thêm mới Lesson
        [HttpPost ("Chapter/{chapterId}/addLesson")]
        public IActionResult AddLesson(int chapterId, AddLessonDto dto)
        {        
            try
            {
                var lesson = new Lesson
                {
                    ChapterId = chapterId,
                    LessonTitle = dto.LessonTitle,
                    LessonContent = dto.LessonContent,
                    Video = dto.Video
                };
                return Ok(_lessonRepository.AddLesson(lesson));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi thêm lesson mới: {ex.Message}");
            }
        }

        // Lấy Lesson theo ID
        [HttpGet("{id}")]
        public IActionResult GetLessonById(int id)
        {
            var lesson = _lessonRepository.GetLessonById(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return Ok(lesson);
        }

        // Lấy Lessons theo ChapterId
        [HttpGet("ByChapter/{chapterId}")]
        public IActionResult GetLessonsByChapterId(int chapterId)
        {
            var lessons = _lessonRepository.GetLessonsByChapterId(chapterId);
            if (lessons == null || !lessons.Any())
            {
                return NotFound("Không tìm thấy lesson nào cho chapter này.");
            }
            return Ok(lessons);
        }

        // Lấy Lessons theo CourseId
        [HttpGet("ByCourse/{courseId}")]
        public IActionResult GetLessonsByCourseId(int courseId)
        {
            var lessons = _lessonRepository.GetLessonsByCourseId(courseId);
            if (lessons == null || !lessons.Any())
            {
                return NotFound("Không tìm thấy lesson nào cho course này.");
            }
            return Ok(lessons);
        }
        // Cập nhật Lesson
        [HttpPut("{id}")]
        public IActionResult UpdateLesson(int id, [FromBody] Lesson lesson)
        {
            if (id != lesson.LessonId)
            {
                return BadRequest("ID không khớp.");
            }

            try
            {
                var updatedLesson = _lessonRepository.UpdateLesson(lesson);
                return Ok(updatedLesson);
            }
            catch (Exception ex)
            {
                return NotFound($"Không tìm thấy lesson với ID: {id}. Lỗi: {ex.Message}");
            }
        }

        // Xóa Lesson
        [HttpDelete("{id}")]
        public IActionResult DeleteLesson(int id)
        {
            try
            {
                _lessonRepository.DeleteLesson(id);
                return Ok($"Lesson với ID: {id} đã được xóa.");
            }
            catch (Exception ex)
            {
                return NotFound($"Không tìm thấy lesson với ID: {id}. Lỗi: {ex.Message}");
            }
        }
    }
}
