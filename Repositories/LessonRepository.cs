using EduhubAPI.Models;

namespace EduhubAPI.Repositories
{
    public class LessonRepository
    {
        private readonly EDUHUBContext _context;
        public LessonRepository(EDUHUBContext context)
        {
            _context = context;
        }
        public IEnumerable<Lesson> GetAllCourses()
        {
            return _context.Set<Lesson>().ToList();
        }

        public Lesson AddLesson(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            _context.SaveChanges();
            return lesson;
        }

        public Lesson GetLessonById(int lessonId)
        {
            return _context.Lessons.FirstOrDefault(c => c.LessonId == lessonId);
        }

        public Lesson UpdateLesson(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            _context.SaveChanges();
            return lesson;
        }

        public void DeleteLesson(int lessonId)
        {
            var lesson = _context.Lessons.Find(lessonId);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Lesson> GetLessonsByChapterId(int chapterId)
        {
            return _context.Lessons.Where(l => l.ChapterId == chapterId).ToList();
        }

        public IEnumerable<Lesson> GetLessonsByCourseId(int courseId)
        {
            return _context.Lessons
                           .Where(l => _context.Chapters
                                               .Where(c => c.CourseId == courseId)
                                               .Select(c => c.ChapterId)
                                               .Contains(l.ChapterId))
                           .ToList();
        }

    }
}
