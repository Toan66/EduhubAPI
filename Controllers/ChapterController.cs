﻿using EduhubAPI.Helpers;
using EduhubAPI.Models;
using EduhubAPI.Repositories;
using EduhubAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly ChapterRepository _chapterRepository;
        private readonly CourseRepository _courseRepository;
        private readonly JwtService _jwtService;

        public ChapterController(ChapterRepository chapterRepository, JwtService jwtService, CourseRepository courseRepository)
        {
            _chapterRepository = chapterRepository;
            _courseRepository = courseRepository;
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

        [HttpDelete("{id}")]
        public IActionResult DeleteChapter(int id)
        {
            try
            {
                _chapterRepository.DeleteChapter(id);
                return Ok($"Chapter with ID: {id} has been successfully deleted.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest($"An error occurred while deleting the chapter: {e.Message}");
            }
        }

        [HttpGet("GetByCourse/{courseId}")]
        public IActionResult GetChaptersByCourseId(int courseId)
        {
            var chapters = _chapterRepository.GetChaptersByCourseId(courseId);
            if (chapters == null || !chapters.Any())
            {
                return NotFound("Chapter not found!");
            }
            return Ok(chapters);
        }
        [HttpGet("{id}/details")]
        public IActionResult GetChapterDetails(int id)
        {

            var chapterDetails = _chapterRepository.GetChapterDetails(id);
            if (chapterDetails == null)
            {
                return NotFound("Chapter not found!");
            }

            return Ok(chapterDetails);
        }

        [HttpGet("{id}/details/edit")]
        public IActionResult GetChapterDetailsEdit(int id)
        {
            var jwt = Request.Cookies["jwt"];
            if (string.IsNullOrEmpty(jwt))
            {
                return Unauthorized();
            }
            var token = _jwtService.Verify(jwt);
            int userId = int.Parse(token.Issuer);

            var chapterDetails = _chapterRepository.GetChapterDetails(id);
            var course = _courseRepository.GetCourseById(chapterDetails.CourseId);

            if (userId != course.TeacherId)
            {
                return Unauthorized("You don't have permission to Edit this Chapter!");
            }
            if (chapterDetails == null)
            {
                return NotFound("Chapter not found!");
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
        [HttpPut("{chapterId}/updateChapterDescription")]
        public IActionResult UpdateChapterDescription(int chapterId, [FromBody] UpdateChapterDescriptionDto dto)
        {
            try
            {
                //var jwt = Request.Cookies["jwt"];
                //if (string.IsNullOrEmpty(jwt))
                //{
                //    return Unauthorized();
                //}
                //var token = _jwtService.Verify(jwt);
                //int userId = int.Parse(token.Issuer);

                var chapter = _chapterRepository.GetChapterById(chapterId);
                if (chapter == null)
                {
                    return NotFound("Chapter dont exist!");
                }
                //else if (chapter.CourseId != userId)
                //{
                //    return Unauthorized("You dont have permission to do this!");
                //}
                chapter.ChapterDescription = dto.ChapterDescription;
                _chapterRepository.UpdateChapter(chapter);
                return Ok("Updated!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("{chapterId}/updateTitle")]
        public IActionResult UpdateChapterTitle(int chapterId, [FromBody] UpdateChapterTitleDto dto)
        {
            try
            {
                var chapter = _chapterRepository.GetChapterById(chapterId);
                if (chapter == null)
                {
                    return NotFound("Chapter dont exist!");
                }
                chapter.ChapterTitle = dto.ChapterTitle;
                _chapterRepository.UpdateChapter(chapter);
                return Ok("Updated!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{chapterId}/completedItems")]
        public IActionResult GetCompletedItemsByChapter(int chapterId)
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

                var completedLessons = _chapterRepository.GetCompletedLessons(userId, chapterId);
                var completedTests = _chapterRepository.GetCompletedTests(userId, chapterId);

                var result = new
                {
                    CompletedLessons = completedLessons,
                    CompletedTests = completedTests
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{chapterId}/completion")]
        public IActionResult GetChapterCompletion(int chapterId)
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

                var completionPercent = _chapterRepository.GetChapterCompletionPercent(userId, chapterId);

                if (completionPercent == null)
                {
                    return NotFound(new { message = "Không tìm thấy thông tin hoàn thành cho chương này." });
                }

                return Ok(new { completionPercent = completionPercent });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}