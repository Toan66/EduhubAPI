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

        public IEnumerable<object> GetCoursesByTeacherIdWithTeacherInfo(int teacherId)
        {
            return _context.Courses.Where(c => c.TeacherId == teacherId).Where(c => c.ApprovalStatus == true)
                .Select(c => new
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
                    Chapters = c.Chapters,
                    Enrollments = c.Enrollments,
                    TeacherInfo = c.Teacher.UserInfos.Select(u => new
                    {
                        FullName = u.FullName,
                        Email = u.Email,
                        DateOfBirth = u.DateOfBirth,
                        Gender = u.Gender,
                        PhoneNumber = u.PhoneNumber,
                        Avatar = u.Avatar,
                        UserAddress = u.UserAddress,
                        UserDescription = u.UserDescription,
                    }).FirstOrDefault(),
                }).ToList();
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
                        }).ToList(),
                        Tests = ch.Tests.Select(t => new Test
                        {
                            TestId = t.TestId,
                            TestTitle = t.TestTitle,
                            TestDescription = t.TestDescription
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
                var teacherInfo = _context.UserInfos
                .Where(t => t.UserId == course.TeacherId)
                .Select(t => new UserInfo
                {
                    UserId = t.UserId,
                    FullName = t.FullName,
                    Email = t.Email,
                    DateOfBirth = t.DateOfBirth,
                    Gender = t.Gender,
                    PhoneNumber = t.PhoneNumber,
                    Avatar = t.Avatar,
                    UserAddress = t.UserAddress,
                    UserDescription = t.UserDescription,
                }).FirstOrDefault();


                return teacherInfo;
            }
            return null;
        }

        public IEnumerable<object> GetCourseReviewsByTeacherId(int teacherId)
        {
            var courses = _context.Courses.Where(c => c.TeacherId == teacherId).Where(c => c.ApprovalStatus == true).Select(c => c.CourseId).ToList();
            var reviewsWithUserInfo = _context.Reviews
            .Where(r => r.CourseId.HasValue && courses.Contains(r.CourseId.Value))
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

    }
}
