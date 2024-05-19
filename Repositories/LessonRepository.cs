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

        public StudentLesson MarkLessonComplete(int userId, int lessonId)
        {
            var existingRecord = _context.StudentLessons.FirstOrDefault(sl => sl.UserId == userId && sl.LessonId == lessonId);

            if (existingRecord != null)
            {
                existingRecord.CompleteDate = DateTime.UtcNow;
            }
            else
            {
                var newRecord = new StudentLesson
                {
                    UserId = userId,
                    LessonId = lessonId,
                    CompleteDate = DateTime.UtcNow
                };
                _context.StudentLessons.Add(newRecord);
                existingRecord = newRecord;
            }

            _context.SaveChanges();
            return existingRecord;
        }

        public void UpdateCompletePercent(int studentId, int chapterId)
        {
            // Lấy tổng số Lesson trong Chapter
            int totalLessons = _context.Lessons.Count(l => l.ChapterId == chapterId);

            // Lấy số Lesson đã hoàn thành bởi học viên trong Chapter
            int completedLessons = _context.StudentLessons
                .Join(_context.Lessons, sl => sl.LessonId, l => l.LessonId, (sl, l) => new { sl, l })
                .Count(joined => joined.l.ChapterId == chapterId && joined.sl.UserId == studentId && joined.sl.CompleteDate != null);

            // Tính tổng số Test trong Chapter
            int totalTests = _context.Tests.Count(t => t.ChapterId == chapterId);

            // Lấy số Test đã hoàn thành bởi học viên trong Chapter dựa trên điều kiện điểm số >= 8
            int completedTests = _context.StudentTestAttempts
                .Join(_context.Tests, sta => sta.TestId, t => t.TestId, (sta, t) => new { sta, t })
                .Count(joined => joined.t.ChapterId == chapterId && joined.sta.UserId == studentId && joined.sta.Score >= 8);

            int totalItems = totalLessons + totalTests;
            int completedItems = completedLessons + completedTests;

            // Đảm bảo không chia cho 0
            decimal completePercent = totalItems > 0 ? (decimal)completedItems / totalItems * 100 : 0;

            // Cập nhật CompletePercent trong bảng StudentChapter
            var studentChapter = _context.StudentChapters.FirstOrDefault(sc => sc.UserId == studentId && sc.ChapterId == chapterId);
            if (studentChapter != null)
            {
                studentChapter.CompletePercent = completePercent;
                _context.SaveChanges();
            }
        }

        public int GetChapterIdByLessonId(int lessonId)
        {
            return _context.Lessons.FirstOrDefault(l => l.LessonId == lessonId).ChapterId;
        }


    }
}
