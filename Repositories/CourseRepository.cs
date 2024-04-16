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
                    Chapters = c.Chapters.Select(ch => new Chapter
                    {
                        ChapterId = ch.ChapterId,
                        ChapterTitle = ch.ChapterTitle,
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
    }
}
