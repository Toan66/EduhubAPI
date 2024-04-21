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
}