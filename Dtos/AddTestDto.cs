namespace EduhubAPI.Dtos
{
    public class AddTestDto
    {
        public string TestTitle { get; set; }
        public string TestDescription { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

    public class QuestionDto
    {
        public string QuestionText { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }

    public class AnswerDto
    {
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
