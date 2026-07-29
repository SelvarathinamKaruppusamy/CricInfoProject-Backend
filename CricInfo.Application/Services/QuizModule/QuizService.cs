public class QuizService
    : IQuizService
{
    private readonly
        IQuizRepository
        _repository;

    public QuizService(
        IQuizRepository repository)
    {
        _repository = repository;
    }

    public async Task<
        List<QuizQuestion>>
        GetQuestionsAsync()
    {
        return await
            _repository
            .GetQuestionsAsync();
    }

    public async Task<int> SubmitQuizAsync(QuizSubmissionDto dto)
    {
        if (dto?.Answers == null || dto.Answers.Count == 0)
        {
            throw new ArgumentException("No answers were submitted.");
        }

        var questions = await _repository.GetQuestionsAsync();

        // Index by Id once, instead of scanning the list for every answer
        var questionsById = questions.ToDictionary(q => q.Id);

        int score = 0;

        foreach (var answer in dto.Answers)
        {
            if (!questionsById.TryGetValue(answer.QuestionId, out var question))
            {
   
                continue;
            }

            if (question.CorrectAnswer == answer.SelectedOption)
            {
                score++;
            }
        }

        await _repository.SaveResultAsync(new QuizResult
        {
            UserName = dto.UserName,
            Score = score,
            CreatedAt = DateTime.UtcNow 
        });

        return score;
    }

    public async Task<
        List<QuizResult>>
        GetLeaderBoardAsync()
    {
        return await
            _repository
            .GetLeaderBoardAsync();
    }
    public async Task<List<QuizQuestion>>
    GetAllQuestionsAsync()
    {
        return await
            _repository
            .GetAllQuestionsAsync();
    }

    public async Task<QuizQuestion?>
        GetQuestionByIdAsync(int id)
    {
        return await
            _repository
            .GetQuestionByIdAsync(id);
    }

    public async Task AddQuestionAsync(
        QuizQuestion question)
    {
        await _repository
            .AddQuestionAsync(question);
    }

    public async Task UpdateQuestionAsync(
        QuizQuestion question)
    {
        await _repository
            .UpdateQuestionAsync(question);
    }

    public async Task DeleteQuestionAsync(
        int id)
    {
        await _repository
            .DeleteQuestionAsync(id);
    }
}