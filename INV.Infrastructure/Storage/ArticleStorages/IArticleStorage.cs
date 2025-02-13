using INV.Domain.Entity.ArticleEntity;
namespace INV.Infrastructure.Storage.ArticleStorages
{
    public interface IArticleStorage
    {
        Task<int> InsertArticle(Article Article);
        Task<List<Article>> SelectAll();
        Task<Article> SelectArticlesByCodeArticle(int CodeArticle);
        Task<List<Article>> SelectArticlesByCodeChapter(int CodeChapter);
        
    }
}