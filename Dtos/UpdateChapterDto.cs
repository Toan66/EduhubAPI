namespace EduhubAPI.Dtos
{
    public class UpdateChapterOrderDto
    {
        public List<ChapterOrder> ChaptersOrder { get; set; }
    }
    
    public class ChapterOrder
    {
        public int ChapterId { get; set; }
        public int NewOrder { get; set; }
    }
}
