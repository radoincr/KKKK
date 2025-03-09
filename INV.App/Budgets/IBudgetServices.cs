using INV.Domain.Entities.Budget;

namespace INV.App.Budgets;

public interface IBudgetService
{
    Task<int> AddArticle(Article Article);
    Task<List<Article>> GetAllArticles();
    Task<Article> GetArticlesByCodeArticle(int CodeArticle);
    Task<List<Article>> GetArticlesByCodeChapter(int CodeChapter);
    Task<int> AddChapter(Chapter Chapter);
    Task<List<Chapter>> GetAllChapitres();
    Task<Chapter> GetChapterByCode(int CodeChapter);
}