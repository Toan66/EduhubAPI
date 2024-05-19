namespace EduhubAPI.Dtos
{
    public class TestSubmissionDto
    {
        public int TestId { get; set; }
        public List<QuestionAnswer> Answers { get; set; }
    }

    public class QuestionAnswer
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
