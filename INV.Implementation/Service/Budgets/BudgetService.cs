using INV.App.Budgets;
using INV.Domain.Entities.Budget;
using INV.Infrastructure.Storage.Budgets;

namespace INV.Implementation.Service.BudgetServices;

public class BudgetService : IBudgetService
{
    private readonly IBudgetStorage budgetStorage;

    public BudgetService(IBudgetStorage budgetStorage)
    {
        this.budgetStorage = budgetStorage;
    }

    public async Task<int> AddArticle(Article Article)
    {
        return await budgetStorage.InsertArticle(Article);
    }


    public async Task<List<Article>> GetAllArticles()
    {
        return await budgetStorage.SelectAllArticles();
    }

    public async Task<Article> GetArticlesByCodeArticle(int CodeArticle)
    {
        return await budgetStorage.SelectArticlesByCodeArticle(CodeArticle);
    }


    public async Task<List<Article>> GetArticlesByCodeChapter(int CodeChapter)
    {
        return await budgetStorage.SelectArticlesByCodeChapter(CodeChapter);
    }

    public async Task<int> AddChapter(Chapter Chapter)
    {
        return await budgetStorage.InsertChapter(Chapter);
    }

    public async Task<List<Chapter>> GetAllChapitres()
    {
        return await budgetStorage.SelectAllChapitres();
    }

    public async Task<Chapter> GetChapterByCode(int CodeChapter)
    {
        return await budgetStorage.SelectChapterByCode(CodeChapter);
    }
}