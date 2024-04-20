using EduhubAPI.Dtos;
using EduhubAPI.Models;
using EduhubAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EduhubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestRepository _repository;

        public TestController(TestRepository repository)
        {
            _repository = repository;
        }
        // Add a new test
        [HttpPost("Chapter/{chapterId}/addTest")]
        public IActionResult AddTest(int chapterId, AddTestDto dto)
        {
            try
            {
                var test = new Test
                {
                    TestTitle = dto.TestTitle,
                    TestDescription = dto.TestDescription,
                    ChapterId = chapterId
                };
                return Ok(_repository.AddTest(test));
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
    }
}
