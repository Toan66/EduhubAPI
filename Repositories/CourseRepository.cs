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

        public IEnumerable<Course> GetAllUnapproveCourses()
        {
            return _context.Set<Course>().Where(c => c.ApprovalStatus == false).ToList();
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

        public UserInfo GetUserDetails(int id)
        {
            return _context.UserInfos.FirstOrDefault(u => u.UserId == id);
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
                    Enrollments = c.Enrollments.Select(e => new Enrollment
                    {
                        EnrollmentId = e.EnrollmentId,
                        UserId = e.EnrollmentId
                    }).ToList(),
                    Chapters = c.Chapters.OrderBy(ch => ch.ChapterOrder).Select(ch => new Chapter
                    {
                        ChapterId = ch.ChapterId,
                        ChapterTitle = ch.ChapterTitle,
                        ChapterDescription = ch.ChapterDescription,
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
                    Expertise = t.Expertise,
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

        //public Course ApproveCourse(int courseId)
        //{
        //    var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);

        //}

        public Enrollment EnrollInCourse(int userId, int courseId)
        {
            var enrollment = new Enrollment
            {
                UserId = userId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            return enrollment;
        }

        public bool IsUserEnrolledInCourse(int userId, int courseId)
        {
            return _context.Enrollments.Any(e => e.UserId == userId && e.CourseId == courseId);
        }

        public void StudentChapterEnroll(int userId, int courseId)
        {
            var chapters = _context.Chapters.Where(c => c.CourseId == courseId).OrderBy(c => c.ChapterOrder).ToList();

            bool isFirstChapter = true;

            foreach (var chapter in chapters)
            {
                var studentChapter = new StudentChapter
                {
                    UserId = userId,
                    ChapterId = chapter.ChapterId,
                    IsCompleted = false,
                    IsUnlocked = isFirstChapter,
                    CompletionDate = null
                };

                _context.StudentChapters.Add(studentChapter);

                isFirstChapter = false;
            }

            _context.SaveChanges();
        }

        public decimal GetCompletePercentage(int userId, int courseId)
        {
            return _context.Enrollments
                           .Where(e => e.UserId == userId && e.CourseId == courseId)
                           .Select(e => e.CompletionPercentage)
                           .FirstOrDefault() ?? 0m;
        }

        public List<int> GetCompletedChaptersByCourseId(int userId, int courseId)
        {
            return _context.StudentChapters
                .Where(sc => sc.UserId == userId && sc.CompletePercent == 100 && sc.Chapter.CourseId == courseId)
                .Select(sc => sc.ChapterId)
                .ToList();
        }

        public List<EnrollmentDetailDto> GetEnrolledCourses(int userId)
        {
            var enrolledCoursesWithEnrollmentInfo = _context.Enrollments
                .Where(e => e.UserId == userId)
                .Select(e => new EnrollmentDetailDto
                {
                    CourseId = e.CourseId,
                    EnrollmentId = e.EnrollmentId,
                    EnrollmentDate = e.EnrollmentDate,
                    CompletionPercentage = e.CompletionPercentage,
                    Course = e.Course
                }).ToList();

            return enrolledCoursesWithEnrollmentInfo;
        }

        public IEnumerable<CourseCardDto> SearchCoursesByTitleAndDescription(string query)
        {
            var courses = (from c in _context.Courses
                           join u in _context.UserInfos on c.TeacherId equals u.UserId
                           join cg in _context.CourseCategories on c.CategoryId equals cg.CourseCategoryId
                           join lv in _context.CourseLevels on c.CourseLevelId equals lv.CourseLevelId
                           where c.CourseName.Contains(query) && c.ApprovalStatus == true
                           select new CourseCardDto()
                           {
                               CourseId = c.CourseId,
                               CourseName = c.CourseName,
                               FeatureImage = c.FeatureImage,
                               AverageRating = c.AverageRating,
                               CoursePrice = c.CoursePrice,
                               CourseCategoryName = cg.CourseCategoryName,
                               FullName = u.FullName,
                               Avatar = u.Avatar,
                               TeacherId = u.UserId,
                               CourseLevelName = lv.CourseLevelName,
                               Chapters = c.Chapters.Select(ch => new ChapterDto
                               {
                                   ChapterId = ch.ChapterId,
                                   CourseId = ch.CourseId,
                               }).ToList(),
                               Enrollments = c.Enrollments
                           }).ToList();

            return courses;

            // return _context.Courses
            //                .Where(c => c.CourseName.Contains(query) || c.CourseDescription.Contains(query))
            //                .Where(c => c.ApprovalStatus == true)
            //                .ToList();
        }

        public IEnumerable<CourseCardDto> GetCoursesByCategoryId(int categoryId)
        {
            var courses = (from c in _context.Courses
                           join u in _context.UserInfos on c.TeacherId equals u.UserId
                           join cg in _context.CourseCategories on c.CategoryId equals cg.CourseCategoryId
                           join lv in _context.CourseLevels on c.CourseLevelId equals lv.CourseLevelId
                           where c.CategoryId == categoryId && c.ApprovalStatus == true
                           select new CourseCardDto()
                           {
                               CourseId = c.CourseId,
                               CourseName = c.CourseName,
                               FeatureImage = c.FeatureImage,
                               AverageRating = c.AverageRating,
                               CoursePrice = c.CoursePrice,
                               CourseCategoryName = cg.CourseCategoryName,
                               FullName = u.FullName,
                               Avatar = u.Avatar,
                               TeacherId = u.UserId,
                               CourseLevelName = lv.CourseLevelName,
                               Chapters = c.Chapters.Select(ch => new ChapterDto
                               {
                                   ChapterId = ch.ChapterId,
                                   CourseId = ch.CourseId,
                               }).ToList(),
                               Enrollments = c.Enrollments
                           }).ToList();
            return courses;
        }

        public IEnumerable<CourseCardDto> GetApprovedCourses()
        {
            var courses = (from c in _context.Courses
                           join u in _context.UserInfos on c.TeacherId equals u.UserId
                           join cg in _context.CourseCategories on c.CategoryId equals cg.CourseCategoryId
                           join lv in _context.CourseLevels on c.CourseLevelId equals lv.CourseLevelId
                           where c.ApprovalStatus == true
                           select new CourseCardDto()
                           {
                               CourseId = c.CourseId,
                               CourseName = c.CourseName,
                               FeatureImage = c.FeatureImage,
                               AverageRating = c.AverageRating,
                               CoursePrice = c.CoursePrice,
                               CourseCategoryName = cg.CourseCategoryName,
                               FullName = u.FullName,
                               Avatar = u.Avatar,
                               TeacherId = u.UserId,
                               CourseLevelName = lv.CourseLevelName,
                               Chapters = c.Chapters.Select(ch => new ChapterDto
                               {
                                   ChapterId = ch.ChapterId,
                                   CourseId = ch.CourseId,
                               }).ToList(),
                               Enrollments = c.Enrollments
                           }).ToList();
            return courses;
        }


    }
}
