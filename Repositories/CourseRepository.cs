using EduhubAPI.Dtos;
using EduhubAPI.Models;

namespace EduhubAPI.Repositories
{
    public class CourseRepository
    {
        private readonly EDUHUBContext _context;
        public CourseRepository(EDUHUBContext context)
        {
            _context = context;
        }
        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Set<Course>().ToList();
        }
        public IEnumerable<CourseCategory> GetAllCoursesCategory()
        {
            return _context.Set<CourseCategory>().ToList();
        }

        public IEnumerable<CourseLevel> GetAllCoursesLevel()
        {
            return _context.Set<CourseLevel>().ToList();
        }

        public Course AddCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
            return course;
        }


        public Course GetCourseById(int courseId)
        {
            return _context.Courses.FirstOrDefault(c => c.CourseId == courseId);
        }

        public Course UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            _context.SaveChanges();
            return course;
        }

        public void DeleteCourse(int courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Course> GetCoursesByCategory(int categoryId)
        {
            return _context.Courses.Where(c => c.CategoryId == categoryId).ToList();
        }

        public static implicit operator CourseRepository(UserRepository v)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> GetCoursesByTeacherId(int teacherId)
        {
            return _context.Courses.Where(c => c.TeacherId == teacherId).ToList();
        }

        public Course GetCourseDetails(int courseId)
        {
            var course = _context.Courses
                .Where(c => c.CourseId == courseId)
                .Select(c => new Course
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    CourseDescription = c.CourseDescription,
                    TeacherId = c.TeacherId,
                    ApprovalStatus = c.ApprovalStatus,
                    CategoryId = c.CategoryId,
                    FeatureImage = c.FeatureImage,
                    CourseLevelId = c.CourseLevelId,
                    CoursePrice = c.CoursePrice,
                    CourseEarn = c.CourseEarn,
                    AverageRating = c.AverageRating,
                    CourseLevel = c.CourseLevel,
                    Category = c.Category,
                    Chapters = c.Chapters.OrderBy(ch => ch.ChapterOrder).Select(ch => new Chapter
                    {
                        ChapterId = ch.ChapterId,
                        ChapterTitle = ch.ChapterTitle,
                        ChapterOrder = ch.ChapterOrder,
                        CourseId = ch.CourseId,
                        Lessons = ch.Lessons.Select(l => new Lesson
                        {
                            LessonId = l.LessonId,
                            LessonTitle = l.LessonTitle,
                            ChapterId = l.ChapterId,
                            LessonContent = l.LessonContent,
                            Video = l.Video
                        }).ToList()
                    }).ToList()
                }).FirstOrDefault();

            return course;
        }

        public IEnumerable<Review> GetCourseReviews(int courseId)
        {
            return _context.Reviews.Where(r => r.CourseId == courseId).ToList();
        }

        public IEnumerable<object> GetCourseReviewsWithUserInfo(int courseId)
        {
            var reviewsWithUserInfo = _context.Reviews
                .Where(r => r.CourseId == courseId)
                .Select(r => new
                {
                    ReviewId = r.ReviewId,
                    CourseId = r.CourseId,
                    UserId = r.UserId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate,
                    User = new
                    {
                        UserInfo = r.User.UserInfos.Select(ui => new
                        {
                            FullName = ui.FullName,
                            Email = ui.Email,
                            DateOfBirth = ui.DateOfBirth,
                            Gender = ui.Gender,
                            PhoneNumber = ui.PhoneNumber,
                            Avatar = ui.Avatar,
                            UserAddress = ui.UserAddress,
                            UserDescription = ui.UserDescription,
                        }).FirstOrDefault()
                    }
                }).ToList();

            return reviewsWithUserInfo;
        }

        public UserInfo GetTeacherByCourseId(int courseId)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);
            if (course != null)
            {
                var teacherDto = _context.Users
                    .Where(u => u.UserId == course.TeacherId)
                    .Select(u => new UserInfo
                    {
                        UserId = u.UserId,
                        FullName = u.UserInfos.Select(ui => ui.FullName).FirstOrDefault(),
                        Email = u.UserInfos.Select(ui => ui.Email).FirstOrDefault(),
                        DateOfBirth = u.UserInfos.Select(ui => ui.DateOfBirth).FirstOrDefault(),
                        Gender = u.UserInfos.Select(ui => ui.Gender).FirstOrDefault(),
                        PhoneNumber = u.UserInfos.Select(ui => ui.PhoneNumber).FirstOrDefault(),
                        Avatar = u.UserInfos.Select(ui => ui.Avatar).FirstOrDefault(),
                        UserAddress = u.UserInfos.Select(ui => ui.UserAddress).FirstOrDefault(),
                        UserDescription = u.UserInfos.Select(ui => ui.UserDescription).FirstOrDefault()
                    }).FirstOrDefault();

                return teacherDto;
            }
            return null;
        }

    }
}
