using EduhubAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace EduhubAPI.Repositories
{
    public class ChapterRepository
    {
        private readonly EDUHUBContext _context;

        public ChapterRepository(EDUHUBContext context)
        {
            _context = context;
        }

        // Add a new chapter
        public Chapter AddChapter(Chapter chapter)
        {
            _context.Chapters.Add(chapter);
            _context.SaveChanges();
            return chapter;
        }

        // Get a single chapter by ID
        public Chapter GetChapterById(int chapterId)
        {
            return _context.Chapters.FirstOrDefault(c => c.ChapterId == chapterId);
        }

        // Get all chapters
        public IEnumerable<Chapter> GetAllChapters()
        {
            return _context.Chapters.ToList();
        }

        public IEnumerable<Chapter> GetChaptersByCourseId(int courseId)
        {
            return _context.Chapters
                .Where(c => c.CourseId == courseId)
                .OrderBy(c => c.ChapterOrder)
                .ToList();
        }

        public Chapter UpdateChapter(Chapter chapter)
        {
            _context.Chapters.Update(chapter);
            _context.SaveChanges();
            return chapter;
        }

        //Delete a chapter
        public void DeleteChapter(int chapterId)
        {
            var chapterToDelete = _context.Chapters.Include(c => c.Lessons).Include(c => c.Tests).FirstOrDefault(c => c.ChapterId == chapterId);
            if (chapterToDelete == null)
            {
                throw new ArgumentException($"Chapter with ID: {chapterId} does not exist.");
            }

            if (chapterToDelete.Lessons.Any() || chapterToDelete.Tests.Any())
            {
                throw new InvalidOperationException($"Cannot delete chapter with ID: {chapterId} because it contains lessons or tests.");
            }

            var affectedChapters = _context.Chapters
                .Where(c => c.CourseId == chapterToDelete.CourseId && c.ChapterOrder > chapterToDelete.ChapterOrder)
                .ToList();

            foreach (var chapter in affectedChapters)
            {
                chapter.ChapterOrder--;
            }

            _context.Chapters.Remove(chapterToDelete);
            _context.SaveChanges();
        }

        //public void DeleteChapter(int chapterId)
        //{
        //    var chapterToDelete = _context.Chapters
        //        .Include(c => c.Lessons)
        //        .Include(c => c.Tests)
        //        .FirstOrDefault(c => c.ChapterId == chapterId);

        //    if (chapterToDelete == null)
        //    {
        //        throw new ArgumentException($"Chapter with ID: {chapterId} does not exist.");
        //    }

        //    // Xóa tất cả Lessons của Chapter
        //    _context.Lessons.RemoveRange(chapterToDelete.Lessons);

        //    // Xóa tất cả Tests của Chapter
        //    _context.Tests.RemoveRange(chapterToDelete.Tests);

        //    // Xóa Chapter sau khi đã xóa Lessons và Tests liên quan
        //    _context.Chapters.Remove(chapterToDelete);
        //    _context.SaveChanges();
        //}

        public Chapter GetChapterDetails(int chapterId)
        {
            var chapter = _context.Chapters
                .Where(ch => ch.ChapterId == chapterId)
                .Select(ch => new Chapter
                {
                    ChapterId = ch.ChapterId,
                    ChapterTitle = ch.ChapterTitle,
                    ChapterDescription = ch.ChapterDescription,
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
                        TestDescription = t.TestDescription,
                        Questions = t.Questions.Select(q => new Question
                        {
                            QuestionId = q.QuestionId,
                        }).ToList(),
                    }).ToList()
                }).FirstOrDefault();
            return chapter;
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

        public IEnumerable<int> GetCompletedLessons(int userId, int chapterId)
        {
            // Adjusted to join with Lessons to filter by ChapterId and check completion for a specific user
            // Now only selecting the LessonId
            return _context.StudentLessons
                .Join(_context.Lessons, sl => sl.LessonId, l => l.LessonId, (sl, l) => new { sl, l })
                .Where(joined => joined.l.ChapterId == chapterId && joined.sl.UserId == userId)
                .Select(joined => joined.l.LessonId)
                .ToList();
        }

        public IEnumerable<int> GetCompletedTests(int userId, int chapterId)
        {
            // Adjusted to join with Tests to filter by ChapterId and check test completion for a specific user
            // Now only selecting the TestId
            return _context.StudentTestAttempts
                .Join(_context.Tests, sta => sta.TestId, t => t.TestId, (sta, t) => new { sta, t })
                .Where(joined => joined.t.ChapterId == chapterId && joined.sta.UserId == userId && joined.sta.Score >= 8) // Assuming a score of 8 or more is considered passing/completion
                .Select(joined => joined.t.TestId)
                .ToList();
        }
        public decimal? GetChapterCompletionPercent(int userId, int chapterId)
        {
            var studentChapter = _context.StudentChapters.FirstOrDefault(sc => sc.UserId == userId && sc.ChapterId == chapterId);
            return studentChapter?.CompletePercent;
        }
    }
}