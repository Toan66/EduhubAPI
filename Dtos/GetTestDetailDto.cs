namespace EduhubAPI.Dtos
{
    public class TestDetailDto
    {
        public int TestId { get; set; }
        public string TestTitle { get; set; }
        public string TestDescription { get; set; }
        public List<QuestionDetailDto> Questions { get; set; }
    }
    public class QuestionDetailDto
    {
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public int QuestionPosition { get; set; }
        public List<AnswerDetailDto> Answers { get; set; }
    }
    public class AnswerDetailDto
    {
        public int AnswerId { get; set; }
        public string AnswerContent { get; set; }
        public bool IsCorrect { get; set; }
        public int AnswerPosition { get; set; }
    }
}
