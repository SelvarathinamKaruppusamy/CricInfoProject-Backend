using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;
public class QuizRepository
    : IQuizRepository
{
    private readonly
        CricDbContext
        _context;

    public QuizRepository(
        CricDbContext context)
    {
        _context = context;
    }

    public async Task<List<QuizQuestion>>
GetQuestionsAsync()
    {
        return await _context.QuizQuestions
            .OrderBy(x => Guid.NewGuid())
            .Take(10)
            .ToListAsync();
    }

    public async Task
        SaveResultAsync(
        QuizResult result)
    {
        _context
            .QuizResults
            .Add(result);

        await _context
            .SaveChangesAsync();
    }

    public async Task<
        List<QuizResult>>
        GetLeaderBoardAsync()
    {
        return await
            _context
            .QuizResults
            .OrderByDescending(
                x => x.Score)
            .ThenBy(
                x => x.CreatedAt)
            .ToListAsync();
    }
    public async Task<List<QuizQuestion>>
    GetAllQuestionsAsync()
    {
        return await _context
            .QuizQuestions
            .ToListAsync();
    }

    public async Task<QuizQuestion?>
        GetQuestionByIdAsync(int id)
    {
        return await _context
            .QuizQuestions
            .FirstOrDefaultAsync(
                x => x.Id == id);
    }

    public async Task AddQuestionAsync(
      QuizQuestion question)
    {
        await _context
            .QuizQuestions
            .AddAsync(question);

        await _context
            .SaveChangesAsync();
    }

    public async Task UpdateQuestionAsync(
        QuizQuestion question)
    {
        _context
            .QuizQuestions
            .Update(question);

        await _context
            .SaveChangesAsync();
    }

    public async Task DeleteQuestionAsync(
        int id)
    {
        var question =
            await _context
            .QuizQuestions
            .FirstOrDefaultAsync(
                x => x.Id == id);

        if (question != null)
        {
            _context
                .QuizQuestions
                .Remove(question);

            await _context
                .SaveChangesAsync();
        }
    }
}