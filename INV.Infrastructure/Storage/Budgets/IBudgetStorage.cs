using INV.Domain.Entities.Budget;

namespace INV.Infrastructure.Storage.Budgets;

public interface IBudgetStorage
{
    Task<int> InsertArticle(Article Article);
    Task<List<Article>> SelectAllArticles();
    Task<Article> SelectArticlesByCodeArticle(int CodeArticle);
    Task<List<Article>> SelectArticlesByCodeChapter(int CodeChapter);
    Task<int> InsertChapter(Chapter Chapter);
    Task<List<Chapter>> SelectAllChapitres();
    Task<Chapter> SelectChapterByCode(int CodeChapter);
}