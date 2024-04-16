using EduhubAPI.Helpers;
using EduhubAPI.Models;
using EduhubAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly ChapterRepository _chapterRepository;
        private readonly JwtService _jwtService;

        public ChapterController(ChapterRepository chapterRepository, JwtService jwtService)
        {
            _chapterRepository = chapterRepository;
            _jwtService = jwtService;
        }

        // Thêm mới một chương
        [HttpPost]
        public IActionResult AddChapter([FromBody] Chapter chapter)
        {
            var newChapter = _chapterRepository.AddChapter(chapter);
            return Created("api/chapter", newChapter);
        }

        // Lấy thông tin chi tiết của một chương theo Id
        [HttpGet("{id}")]
        public IActionResult GetChapterById(int id)
        {
            var chapter = _chapterRepository.GetChapterById(id);
            if (chapter == null)
            {
                return NotFound();
            }
            return Ok(chapter);
        }

        // Cập nhật thông tin của một chương
        [HttpPut("{id}")]
        public IActionResult UpdateChapter(int id, [FromBody] Chapter chapter)
        {
            if (id != chapter.ChapterId)
            {
                return BadRequest();
            }

            var updatedChapter = _chapterRepository.UpdateChapter(chapter);
            if (updatedChapter == null)
            {
                return NotFound();
            }
            return Ok(updatedChapter);
        }

        // Xóa một chương
        [HttpDelete("{id}")]
        public IActionResult DeleteChapter(int id)
        {
            _chapterRepository.DeleteChapter(id);
            return NoContent();
        }

        [HttpGet("GetByCourse/{courseId}")]
        public IActionResult GetChaptersByCourseId(int courseId)
        {
            var chapters = _chapterRepository.GetChaptersByCourseId(courseId);
            if (chapters == null || !chapters.Any())
            {
                return NotFound("Không tìm thấy chapter nào cho khóa học này.");
            }
            return Ok(chapters);
        }
    }
}