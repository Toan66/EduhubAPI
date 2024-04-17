namespace EduhubAPI.Dtos
{
    public class AddLessonDto
    {
        public string LessonTitle { get; set; } = null!;
        public string? LessonContent { get; set; }
        public string? Video { get; set; }
    }
}
