namespace EduhubAPI.Dtos
{
    public class AddCourseDto
    {
        public string CourseName { get; set; } = null!;
        public string? CourseDescription { get; set; }
        public int CategoryId { get; set; }
        public string? FeatureImage { get; set; }
    }
}
