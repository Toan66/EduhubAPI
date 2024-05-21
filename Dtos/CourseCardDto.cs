using EduhubAPI.Models;

namespace EduhubAPI.Dtos
{
    public class CourseCardDto
    {
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
        public List<ChapterDto> Chapters { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public string CourseName { get; set; } = null!;
        public string? FeatureImage { get; set; }
        public decimal? AverageRating { get; set; }
        public int? CoursePrice { get; set; }
        public string CourseCategoryName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Avatar { get; set; }
        public string CourseLevelName { get; set; } = null!;

    }
    public class ChapterDto
    {
        public int ChapterId { get; set; }
        public int CourseId { get; set; }
    }
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
    }
}
