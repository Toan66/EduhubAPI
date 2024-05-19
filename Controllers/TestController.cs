using EduhubAPI.Dtos;
using EduhubAPI.Models;
using EduhubAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using EduhubAPI.Helpers;

namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestRepository _repository;
        private readonly JwtService _jwtService;


        public TestController(TestRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }
        // Add a new test
        [HttpPost("Chapter/{chapterId}/addTest")]
        public IActionResult AddTest(int chapterId, [FromBody] AddTestDto dto)
        {
            try
            {
                var test = new Test
                {
                    TestTitle = dto.TestTitle,
                    TestDescription = dto.TestDescription,
                    ChapterId = chapterId,
                    Questions = new List<Question>()
                };

                int questionPosition = 1; // Start position for questions
                foreach (var questionDto in dto.Questions)
                {
                    var question = new Question
                    {
                        QuestionContent = questionDto.QuestionText,
                        QuestionPosition = questionPosition++,
                        Answers = new List<Answer>()
                    };

                    int answerPosition = 1; // Start position for answers
                    foreach (var answerDto in questionDto.Answers)
                    {
                        question.Answers.Add(new Answer
                        {
                            AnswerContent = answerDto.AnswerText,
                            IsCorrect = answerDto.IsCorrect,
                            AnswerPosition = answerPosition++
                        });
                    }

                    test.Questions.Add(question);
                }

                _repository.AddTest(test); // Assuming this method handles saving the test and its related entities correctly

                return Ok(); // Or return a more appropriate response as needed
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Get a test by ID
        [HttpGet("{id}")]
        public IActionResult GetTestById(int id)
        {
            var test = _repository.GetTestById(id);
            if (test == null)
            {
                return NotFound();
            }
            return Ok(test);
        }

        // Update a test
        [HttpPut("{id}")]
        public IActionResult UpdateTest(int id, [FromBody] Test test)
        {
            var updatedTest = _repository.UpdateTest(id, test);
            if (updatedTest == null)
            {
                return NotFound();
            }
            return Ok(updatedTest);
        }

        // Delete a test
        [HttpDelete("{id}")]
        public IActionResult DeleteTest(int id)
        {
            var success = _repository.DeleteTest(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Add a question to a test
        [HttpPost("{testId}/questions")]
        public IActionResult AddQuestionToTest(int testId, [FromBody] Question question)
        {
            var addedQuestion = _repository.AddQuestionToTest(testId, question);
            return Ok(addedQuestion);
        }

        // Get questions by test ID
        [HttpGet("{testId}/questions")]
        public IActionResult GetQuestionsByTestId(int testId)
        {
            var questions = _repository.GetQuestionsByTestId(testId);
            return Ok(questions);
        }

        // Add an answer to a question
        [HttpPost("questions/{questionId}/answers")]
        public IActionResult AddAnswerToQuestion(int questionId, [FromBody] Answer answer)
        {
            var addedAnswer = _repository.AddAnswerToQuestion(questionId, answer);
            return Ok(addedAnswer);
        }

        // Get answers by question ID
        [HttpGet("questions/{questionId}/answers")]
        public IActionResult GetAnswersByQuestionId(int questionId)
        {
            var answers = _repository.GetAnswersByQuestionId(questionId);
            return Ok(answers);
        }
        [HttpGet("{id}/details")]
        public IActionResult GetTestDetails(int id)
        {
            var test = _repository.GetTestDetails(id);
            if (test == null)
            {
                return NotFound("Test not found.");
            }

            var testDetailDto = new TestDetailDto
            {
                TestId = test.TestId,
                TestTitle = test.TestTitle,
                TestDescription = test.TestDescription,
                Questions = test.Questions.Select(q => new QuestionDetailDto
                {
                    QuestionId = q.QuestionId,
                    QuestionContent = q.QuestionContent,
                    QuestionPosition = q.QuestionPosition ?? 1,
                    Answers = q.Answers.Select(a => new AnswerDetailDto
                    {
                        AnswerId = a.AnswerId,
                        AnswerContent = a.AnswerContent,
                        IsCorrect = a.IsCorrect,
                        AnswerPosition = a.AnswerPosition ?? 1
                    }).ToList()
                }).ToList()
            };

            return Ok(testDetailDto);
        }
        [HttpPost("submitTest")]
        public IActionResult SubmitTest([FromBody] TestSubmissionDto submission)
        {
            try
            {
                var jwt = Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt))
                {
                    return Unauthorized("No token provided.");
                }

                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);

                // Calculate the score based on the submitted answers
                var score = _repository.CalculateTestScore(submission);

                var testAttempt = new StudentTestAttempt
                {
                    TestId = submission.TestId,
                    UserId = userId,
                    AttemptDate = DateTime.UtcNow,
                    Score = score
                };

                _repository.SaveTestAttempt(testAttempt);

                int chapterId = _repository.GetChapterIdByTestId(submission.TestId);

                _repository.UpdateCompletePercent(userId, chapterId);

                return Ok(new { Score = score, Message = "Test attempt recorded successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error submitting test: {ex.Message}");
            }
        }

    }
}
