using CricInfo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/quiz")]
public class QuizController : ControllerBase
{
    private readonly IQuizService _service;

    public QuizController(
        IQuizService service)
    {
        _service = service;
    }

    // USER SIDE

    [HttpGet]
    public async Task<IActionResult>
        GetQuestions()
    {
        return Ok(
            await _service
                .GetQuestionsAsync());
    }
    [HttpPost]
    public async Task<IActionResult>
        SubmitQuiz(
        QuizSubmissionDto dto)
    {
        var score =
            await _service
                .SubmitQuizAsync(dto);

        return Ok(score);
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult>
        GetLeaderBoard()
    {
        return Ok(
            await _service
                .GetLeaderBoardAsync());
    }


    // ADMIN SIDE

    [HttpGet("all")]
    public async Task<IActionResult>
        GetAllQuestions()
    {
        return Ok(
            await _service
                .GetAllQuestionsAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetQuestionById(
        int id)
    {
        var question =
            await _service
                .GetQuestionByIdAsync(id);

        if (question == null)
        {
            return NotFound(
                "Question not found.");
        }

        return Ok(question);
    }
    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult>
 AddQuestion(
     QuizQuestion question)
    {
        await _service
            .AddQuestionAsync(question);

        return Ok(question);
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult>
        UpdateQuestion(
        int id,
        QuizQuestion question)
    {
        question.Id = id;

        await _service
            .UpdateQuestionAsync(question);

        return Ok(
            "Question updated successfully.");
    }
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult>
        DeleteQuestion(
        int id)
    {
        await _service
            .DeleteQuestionAsync(id);

        return Ok(
            "Question deleted successfully.");
    }
}