using EduhubAPI.Models;


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

        // Delete a chapter
        public void DeleteChapter(int chapterId)
        {
            var chapterToDelete = _context.Chapters.Find(chapterId);
            if (chapterToDelete != null)
            {
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
        }

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
                    }).ToList()
                }).FirstOrDefault();
            return chapter;
        }
    }
}