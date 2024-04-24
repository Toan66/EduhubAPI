namespace EduhubAPI.Dtos
{
    public class UpdateChapterOrderDto
    {
        public List<ChapterOrderDto> ChaptersOrder { get; set; }
    }

    public class ChapterOrderDto
    {
        public int ChapterId { get; set; }
        public int NewOrder { get; set; }
    }

    public class UpdateChapterDescriptionDto
    {
        public string? ChapterDescription { get; set; }
    }

    public class UpdateChapterTitleDto
    {
        public string ChapterTitle { get; set; } = null!;
    }

}