public interface IQuizService
{
    Task<List<QuizQuestion>>
        GetQuestionsAsync();

    Task<int> SubmitQuizAsync(
        QuizSubmissionDto dto);

    Task<List<QuizResult>>
        GetLeaderBoardAsync();
    Task<List<QuizQuestion>> GetAllQuestionsAsync();

    Task<QuizQuestion?> GetQuestionByIdAsync(int id);

    Task AddQuestionAsync(QuizQuestion question);

    Task UpdateQuestionAsync(QuizQuestion question);

    Task DeleteQuestionAsync(int id);
}