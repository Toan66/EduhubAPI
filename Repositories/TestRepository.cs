using EduhubAPI.Models;


namespace EduhubAPI.Repositories
{
    public class TestRepository
    {
        private readonly EDUHUBContext _context;

        public TestRepository(EDUHUBContext context)
        {
            _context = context;
        }

        // Add a new test
        public Test AddTest(Test test)
        {
            _context.Tests.Add(test);
            _context.SaveChanges();
            return test;
        }

        // Get a test by ID
        public Test GetTestById(int testId)
        {
            return _context.Tests.FirstOrDefault(t => t.TestId == testId);
        }

        
        // Update a test
        public Test UpdateTest(int testId, Test updatedTest)
        {
            var test = _context.Tests.FirstOrDefault(t => t.TestId == testId);
            if (test != null)
            {
                test.TestTitle = updatedTest.TestTitle;
                test.TestDescription = updatedTest.TestDescription;
                test.ChapterId = updatedTest.ChapterId;
                // Update other fields as necessary

                _context.SaveChanges();
            }
            return test;
        }

        // Delete a test
        public bool DeleteTest(int testId)
        {
            var test = _context.Tests.FirstOrDefault(t => t.TestId == testId);
            if (test != null)
            {
                _context.Tests.Remove(test);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        // Add a question to a test
        public Question AddQuestionToTest(int testId, Question question)
        {
            var test = _context.Tests.FirstOrDefault(t => t.TestId == testId);
            if (test != null)
            {
                question.TestId = testId;
                _context.Questions.Add(question);
                _context.SaveChanges();
            }
            return question;
        }
        
        // Get questions by test ID
        public IEnumerable<Question> GetQuestionsByTestId(int testId)
        {
            return _context.Questions.Where(q => q.TestId == testId).ToList();
        }
        
        // Update a question
        public Question UpdateQuestion(int questionId, Question updatedQuestion)
        {
            var question = _context.Questions.FirstOrDefault(q => q.QuestionId == questionId);
            if (question != null)
            {
                question.QuestionContent = updatedQuestion.QuestionContent;
                // Update other fields as necessary
                _context.SaveChanges();
            }
            return question;
        }
        
        // Delete a question
        public bool DeleteQuestion(int questionId)
        {
            var question = _context.Questions.FirstOrDefault(q => q.QuestionId == questionId);
            if (question != null)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        
        // Add an answer to a question
        public Answer AddAnswerToQuestion(int questionId, Answer answer)
        {
            var question = _context.Questions.FirstOrDefault(q => q.QuestionId == questionId);
            if (question != null)
            {
                answer.QuestionId = questionId;
                _context.Answers.Add(answer);
                _context.SaveChanges();
            }
            return answer;
        }
        
        // Get answers by question ID
        public IEnumerable<Answer> GetAnswersByQuestionId(int questionId)
        {
            return _context.Answers.Where(a => a.QuestionId == questionId).ToList();
        }
        
        // Update an answer
        public Answer UpdateAnswer(int answerId, Answer updatedAnswer)
        {
            var answer = _context.Answers.FirstOrDefault(a => a.AnswerId == answerId);
            if (answer != null)
            {
                answer.AnswerContent = updatedAnswer.AnswerContent;
                answer.IsCorrect = updatedAnswer.IsCorrect;
                // Update other fields as necessary
                _context.SaveChanges();
            }
            return answer;
        }
        
        // Delete an answer
        public bool DeleteAnswer(int answerId)
        {
            var answer = _context.Answers.FirstOrDefault(a => a.AnswerId == answerId);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}