using EduhubAPI.Helpers;
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
        [HttpGet("category")]
        public IActionResult GetCategory()
        {
            var courses = _courseRepository.GetAllCoursesCategory();
            return Ok(courses);
        }
        [HttpGet("level")]
        public IActionResult GetLevel()
        {
            var courses = _courseRepository.GetAllCoursesLevel();
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
                    FeatureImage = addCourse.FeatureImage,
                    CourseLevelId = addCourse.CourseLevelId,
                    CourseEarn = addCourse.CourseEarn,
                    CoursePrice = addCourse.CoursePrice
                };

                return Ok(_courseRepository.AddCourse(course));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] UpdateCourseDto updateCourseDto)
        {
            var course = _courseRepository.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }

            course.CourseName = updateCourseDto.CourseName;
            course.CourseDescription = updateCourseDto.CourseDescription;

            _courseRepository.UpdateCourse(course);
            return NoContent();
        }
        [HttpPut("{id}/updateName")]
        public IActionResult UpdateCourseName(int id, [FromBody] UpdateCourseNameDto updateCourseNameDto)
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

                var course = _courseRepository.GetCourseById(id);
                if (course == null)
                {
                    return NotFound("Khóa học không tồn tại.");
                }
                else if (course.TeacherId != userId)
                {
                    return Unauthorized("Bạn không có quyền cập nhật tên khóa học này.");
                }
                course.CourseName = updateCourseNameDto.CourseName;
                _courseRepository.UpdateCourse(course);
                return Ok("Tên khóa học đã được cập nhật.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPut("{id}/updateImage")]
        public IActionResult UpdateCourseImage(int id, [FromBody] UpdateCourseImageDto updateCourseImageDto)
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

                var course = _courseRepository.GetCourseById(id);
                if (course == null)
                {
                    return NotFound("Khóa học không tồn tại.");
                }
                else if (course.TeacherId != userId)
                {
                    return Unauthorized("Bạn không có quyền cập nhật tên khóa học này.");
                }
                course.FeatureImage = updateCourseImageDto.FeatureImage;
                _courseRepository.UpdateCourse(course);
                return Ok("Ảnh đại diện khóa học đã được cập nhật.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpPut("{id}/updateDescription")]
        public IActionResult UpdateCourseDescription(int id, [FromBody] UpdateCourseDescriptionDto updateCourseDescriptionDto)
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

                var course = _courseRepository.GetCourseById(id);
                if (course == null)
                {
                    return NotFound("Khóa học không tồn tại.");
                }
                else if (course.TeacherId != userId)
                {
                    return Unauthorized("Bạn không có quyền cập nhật tên khóa học này.");
                }
                course.CourseDescription = updateCourseDescriptionDto.CourseDescription;
                _courseRepository.UpdateCourse(course);
                return Ok("Mô tả khóa học đã được cập nhật.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("{id}/updateCategory")]
        public IActionResult UpdateCourseCategory(int id, [FromBody] UpdateCourseCategoryDto updateCourseCategoryDto)
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

                var course = _courseRepository.GetCourseById(id);
                if (course == null)
                {
                    return NotFound("Khóa học không tồn tại.");
                }
                else if (course.TeacherId != userId)
                {
                    return Unauthorized("Bạn không có quyền cập nhật tên khóa học này.");
                }
                course.CategoryId = updateCourseCategoryDto.CategoryId;
                _courseRepository.UpdateCourse(course);
                return Ok("Danh mục khóa học đã được cập nhật.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("{id}/updateLevel")]
        public IActionResult UpdateCourseLevel(int id, [FromBody] UpdateCourseLevelDto dto)
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
        
                var course = _courseRepository.GetCourseById(id);
                if (course == null)
                {
                    return NotFound("Khóa học không tồn tại.");
                }
                else if (course.TeacherId != userId)
                {
                    return Unauthorized("Bạn không có quyền cập nhật cấp độ của khóa học này.");
                }
                course.CourseLevelId = dto.CourseLevelId;
                _courseRepository.UpdateCourse(course);
                return Ok("Cấp độ khóa học đã được cập nhật.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("{id}/updateEarn")]
        public IActionResult UpdateCourseEarn(int id, [FromBody] UpdateCourseEarnDto dto)
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
        
                var course = _courseRepository.GetCourseById(id);
                if (course == null)
                {
                    return NotFound("Khóa học không tồn tại.");
                }
                else if (course.TeacherId != userId)
                {
                    return Unauthorized("Bạn không có quyền cập nhật thu nhập từ khóa học này.");
                }
                course.CourseEarn = dto.CourseEarn;
                _courseRepository.UpdateCourse(course);
                return Ok("Thu nhập từ khóa học đã được cập nhật.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}/updatePrice")]
        public IActionResult UpdateCoursePrice(int id, [FromBody] UpdateCoursePriceDto dto)
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
        
                var course = _courseRepository.GetCourseById(id);
                if (course == null)
                {
                    return NotFound("Khóa học không tồn tại.");
                }
                else if (course.TeacherId != userId)
                {
                    return Unauthorized("Bạn không có quyền cập nhật giá của khóa học này.");
                }
                course.CoursePrice = dto.CoursePrice;
                _courseRepository.UpdateCourse(course);
                return Ok("Giá khóa học đã được cập nhật.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
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

                var course = _courseRepository.GetCourseById(id);
                if (course == null)
                {
                    return NotFound("Course does not exist.");
                }
                else if (course.TeacherId != userId)
                {
                    return Unauthorized("You don't have permission to do this.");
                }
                _courseRepository.DeleteCourse(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
        [HttpGet("{id}/details/edit")]
        public IActionResult GetCourseDetailEdit(int id)
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

                var courseDetails = _courseRepository.GetCourseDetails(id);
                if (courseDetails == null)
                {
                    return NotFound("Khóa học không tồn tại.");
                }
                else if (courseDetails.TeacherId != userId)
                {
                    return Unauthorized("Bạn không có quyền cập nhật tên khóa học này.");
                }
                return Ok(courseDetails);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

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

        [HttpGet("{id}/reviews")]
        public IActionResult GetCourseReviews(int id)
        {
            var reviews = _courseRepository.GetCourseReviewsWithUserInfo(id);
            if (reviews == null || !reviews.Any())
            {
                return NotFound("Not found.");
            }
            return Ok(reviews);
        }



        [HttpGet("{id}/teacher")]
        public IActionResult GetTeacherByCourseId(int id)
        {
            var teacher = _courseRepository.GetTeacherByCourseId(id);
            if (teacher == null)
            {
                return NotFound("Not found.");
            }
            return Ok(teacher);
        }
    }
}
