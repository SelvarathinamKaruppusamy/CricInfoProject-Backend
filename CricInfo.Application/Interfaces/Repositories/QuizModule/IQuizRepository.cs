using CricInfo.Domain.Entities;

public interface IQuizRepository
{
    Task<List<QuizQuestion>> GetQuestionsAsync();

    Task SaveResultAsync(
        QuizResult result);

    Task<List<QuizResult>>
        GetLeaderBoardAsync();
    Task<List<QuizQuestion>> GetAllQuestionsAsync();

    Task<QuizQuestion?> GetQuestionByIdAsync(int id);

    Task AddQuestionAsync(QuizQuestion question);

    Task UpdateQuestionAsync(QuizQuestion question);

    Task DeleteQuestionAsync(int id);
}