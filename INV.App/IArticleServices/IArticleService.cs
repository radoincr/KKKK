using INV.Domain.Entities.Budget;

namespace INV.App.IArticleServices
{
    public interface IArticleService
    {
        Task<int> AddArticle(Article article);
        Task<List<Article>> GetAllArticles();
        Task<Article>  GetArticlesByCodeArticle(int CodeArticle);
        Task<List<Article>> GetArticlesByCodeChapter(int CodeChapter);
    }
}