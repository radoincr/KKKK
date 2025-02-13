using INV.Domain.Entity.ArticleEntity;
using INV.Domain.Entity.ArticleEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using INV.Domain.Entity.ChapterEntity;
namespace INV.Infrastructure.Storage.ArticleStorages
{
    public class ArticleStorage:IArticleStorage
    {
        private readonly string _connectionString;
        public ArticleStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }
        private const string insertArticleQuery = "INSERT INTO ARTICLE (CodeArticle,Name,CodeChapter) VALUES (@CodeArticle,@Name,@CodeChapter)";
        private const string selectAll = "SELECT *FROM Article";
        private const string selectArticleByCodeArticle = "Select * From Article where CodeArticle=@CodeArticle ";
        private const string selectArticleByCodeChapter = "Select * From Article where CodeChapter=@CodeChapter ";
       public async Task<int> InsertArticle(Article article)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(insertArticleQuery, sqlConnection);
            cmd.Parameters.AddWithValue("@CodeArticle", article.CodeArticle);
            cmd.Parameters.AddWithValue("@Name",article.Name);
            cmd.Parameters.AddWithValue("@CodeChapter", article.CodeArticle);
            await sqlConnection.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }
      public  async Task<List<Article>> SelectAll()
        {
            var articles = new List<Article>();
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(selectAll, sqlConnection);
            await sqlConnection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                articles.Add(ArticleDataReader(reader));
            }
            return articles;
        }
      public  async Task<Article> SelectArticlesByCodeArticle(int CodeArticle)
        {
            var Articles = new Article();
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(selectArticleByCodeArticle, sqlConnection);
            cmd.Parameters.AddWithValue("@CodeArticle", CodeArticle);
            await sqlConnection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            Articles = (ArticleDataReader(reader));
            return Articles;
        }
       public async Task<List<Article>> SelectArticlesByCodeChapter(int CodeChapter)
        {
            var articles = new List<Article>();
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(selectArticleByCodeChapter, sqlConnection);
            cmd.Parameters.AddWithValue("@CodeArticle", CodeChapter);
            await sqlConnection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                articles.Add(ArticleDataReader(reader));
            }
            return articles;
        }
        private static Article ArticleDataReader(SqlDataReader reader)
        {
            return new Article
            {
                CodeArticle = (int)reader["CodeArticle"],
                Name = reader["Name"].ToString(),
                CodeChapter = (int)reader["CodeChapter"],
            };
        }
    }
}