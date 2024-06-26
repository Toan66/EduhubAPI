namespace EduhubAPI.Dtos
{
    public class UpdateCourseDto
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string? FeatureImage { get; set; }
    }
    public class UpdateCourseNameDto
    {
        public string CourseName { get; set; }
    }

    public class UpdateCourseImageDto
    {
        public string FeatureImage { get; set; }
    }
    public class UpdateCourseDescriptionDto
    {
        public string CourseDescription { get; set; }
    }
    public class UpdateCourseCategoryDto
    {
        public int CategoryId { get; set; }
    }
    public class UpdateCourseLevelDto
    {
        public int CourseLevelId { get; set; }
    }

    public class UpdateCoursePriceDto
    {
        public int CoursePrice { get; set; }
    }

    public class UpdateCourseEarnDto
    {
        public int CourseEarn { get; set; }
    }
}
