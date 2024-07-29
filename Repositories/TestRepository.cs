using EduhubAPI.Models;
using Microsoft.EntityFrameworkCore;
using EduhubAPI.Dtos;

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
                // Retrieve all related questions
                var questions = _context.Questions.Where(q => q.TestId == testId).ToList();

                // For each question, retrieve and delete all related answers
                foreach (var question in questions)
                {
                    var answers = _context.Answers.Where(a => a.QuestionId == question.QuestionId).ToList();
                    _context.Answers.RemoveRange(answers);
                }

                // Delete all questions related to the test
                _context.Questions.RemoveRange(questions);

                // Delete the test itself
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
        public Test GetTestDetails(int testId)
        {
            // Assuming you have navigation properties set up correctly in your models
            // This will fetch a test and include its related questions and answers
            return _context.Tests
                .Where(t => t.TestId == testId)
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefault();
        }
        public decimal CalculateTestScore(TestSubmissionDto submission)
        {
            int totalQuestions = _context.Questions.Count(q => q.TestId == submission.TestId);
            if (totalQuestions == 0) return 0; // Tránh chia cho 0

            int correctAnswers = 0;
            foreach (var answer in submission.Answers)
            {
                var correctAnswer = _context.Answers.FirstOrDefault(a => a.QuestionId == answer.QuestionId && a.IsCorrect);
                if (correctAnswer != null && correctAnswer.AnswerId == answer.AnswerId)
                {
                    correctAnswers++;
                }
            }

            decimal scorePerQuestion = 10m / totalQuestions;
            decimal totalScore = correctAnswers * scorePerQuestion;

            return Math.Round(totalScore, 2);
        }

        public void SaveTestAttempt(StudentTestAttempt testAttempt)
        {
            _context.StudentTestAttempts.Add(testAttempt);
            _context.SaveChanges();
        }

        public void UpdateCompletePercent(int studentId, int chapterId)
        {
            // Lấy tổng số Lesson trong Chapter
            int totalLessons = _context.Lessons.Count(l => l.ChapterId == chapterId);

            // Lấy số Lesson đã hoàn thành bởi học viên trong Chapter
            int completedLessons = _context.StudentLessons
                .Join(_context.Lessons, sl => sl.LessonId, l => l.LessonId, (sl, l) => new { sl, l })
                .Count(joined => joined.l.ChapterId == chapterId && joined.sl.UserId == studentId && joined.sl.CompleteDate != null);

            // Tính tổng số Test trong Chapter
            int totalTests = _context.Tests.Count(t => t.ChapterId == chapterId);

            // Lấy số Test đã hoàn thành bởi học viên trong Chapter dựa trên điều kiện điểm số >= 8
            int completedTests = _context.StudentTestAttempts
                .Join(_context.Tests, sta => sta.TestId, t => t.TestId, (sta, t) => new { sta, t })
                .Count(joined => joined.t.ChapterId == chapterId && joined.sta.UserId == studentId && joined.sta.Score >= 8);

            int totalItems = totalLessons + totalTests;
            int completedItems = completedLessons + completedTests;

            // Đảm bảo không chia cho 0
            decimal completePercent = totalItems > 0 ? (decimal)completedItems / totalItems * 100 : 0;

            if (completePercent > 100) { completePercent = 100; };

            // Cập nhật CompletePercent trong bảng StudentChapter
            var studentChapter = _context.StudentChapters.FirstOrDefault(sc => sc.UserId == studentId && sc.ChapterId == chapterId);
            if (studentChapter != null)
            {
                studentChapter.CompletePercent = completePercent;
                _context.SaveChanges();
            }
        }

        public int GetChapterIdByTestId(int testId)
        {
            return _context.Tests.FirstOrDefault(t => t.TestId == testId).ChapterId;
        }

        public StudentTestAttempt GetTestAttemptDetails(int testAttemptId)
        {
            var testAttempt = _context.StudentTestAttempts.Where(sta => sta.TestAttemptId == testAttemptId).FirstOrDefault() ?? throw new Exception("Test attempt not found.");
            return testAttempt;
        }
    }
}