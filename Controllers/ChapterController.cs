﻿using EduhubAPI.Helpers;
using EduhubAPI.Models;
using EduhubAPI.Repositories;
using EduhubAPI.Dtos;
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

        [HttpPost("Course/{courseId}/addChapter")]
        public IActionResult AddChapter(int courseId, AddChapterDto dto)
        {

            try
            {
                var currentChapterCount = _chapterRepository.GetChaptersByCourseId(courseId).Count();
                var chapter = new Chapter
                {
                    ChapterTitle = dto.ChapterTitle,
                    ChapterDescription = dto.ChapterDescription,
                    ChapterOrder = currentChapterCount + 1,
                    CourseId = courseId
                };
                return Ok(_chapterRepository.AddChapter(chapter));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
        [HttpGet("{id}/details")]
        public IActionResult GetChapterDetails(int id)
        {
            var chapterDetails = _chapterRepository.GetChapterDetails(id);
            if (chapterDetails == null)
            {
                return NotFound("Không tìm thấy chương.");
            }

            return Ok(chapterDetails);
        }

        [HttpPut("Course/{courseId}/updateChapterOrder")]
        public IActionResult UpdateChapterOrder(int courseId, [FromBody] UpdateChapterOrderDto updateOrderDto)
        {
            try
            {
                if (updateOrderDto.ChaptersOrder == null || !updateOrderDto.ChaptersOrder.Any())
                {
                    return BadRequest("Invalid chapter list.");
                }

                var courseChapters = _chapterRepository.GetChaptersByCourseId(courseId);
                if (courseChapters == null || !courseChapters.Any())
                {
                    return NotFound($"No chapters found for course ID: {courseId}.");
                }

                foreach (var chapterOrder in updateOrderDto.ChaptersOrder)
                {
                    var chapter = courseChapters.FirstOrDefault(c => c.ChapterId == chapterOrder.ChapterId);
                    if (chapter == null)
                    {
                        return NotFound($"Chapter with ID: {chapterOrder.ChapterId} not found in course ID: {courseId}.");
                    }
                    chapter.ChapterOrder = chapterOrder.NewOrder;
                    _chapterRepository.UpdateChapter(chapter);
                }

                return Ok("Chapter order updated successfully for course ID: " + courseId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}