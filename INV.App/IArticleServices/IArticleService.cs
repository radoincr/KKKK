using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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