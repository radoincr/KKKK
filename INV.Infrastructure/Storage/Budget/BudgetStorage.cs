using INV.Domain.Entities.Budget;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.Budget
{
    public class BudgetStorage : IBudgetStorage { 
        private readonly string _connectionString;
        public BudgetStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }
    
        private const string insertArticleQuery = "INSERT INTO ARTICLE (CodeArticle,Name,CodeChapter) VALUES (@aCodeArticle,@aName,@aCodeChapter)";
        private const string selectAllArticlesQuery = "SELECT *FROM Article";
        private const string selectArticleByCodeArticle = "Select * From Article where CodeArticle=@aCodeArticle ";
        private const string selectArticleByCodeChapter = "Select * From Article where CodeChapter=@aCodeChapter ";
        
        private const string insertChapterQuery = "INSERT INTO Chapter (CodeChapter,Name) VALUES (@aCodeChapter,@aName)";
        private const string selectAllChapitresQuery = "SELECT * FROM Chapter";
        private const string selectChaptersByCode = "Select * From Chapter where CodeChapter=@aCodeChapter ";
        public async Task<int> InsertArticle(Article article)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(insertArticleQuery, sqlConnection);
            cmd.Parameters.AddWithValue("@aCodeArticle", article.CodeArticle);
            cmd.Parameters.AddWithValue("@aName",article.Name);
            cmd.Parameters.AddWithValue("@aCodeChapter", article.CodeArticle);
            await sqlConnection.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }
        public  async Task<List<Article>> SelectAllArticles()
        {
            var articles = new List<Article>();
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(selectAllArticlesQuery, sqlConnection);
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
            cmd.Parameters.AddWithValue("@aCodeArticle", CodeArticle);
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
            cmd.Parameters.AddWithValue("@aCodeArticle", CodeChapter);
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
        
        
        public async Task<int> InsertChapter(Chapter Chapter)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(insertChapterQuery, sqlConnection);
            cmd.Parameters.AddWithValue("@aCodeChapter", Chapter.CodeChapter);
            cmd.Parameters.AddWithValue("@aName", Chapter.Name);
            await sqlConnection.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }
        public async Task<List<Chapter>> SelectAllChapitres()
        {
            var chapters = new List<Chapter>();
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(selectAllChapitresQuery, sqlConnection);
            await sqlConnection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                chapters.Add(ChapterDataReader(reader));
            }
            return chapters;
        }
        public async Task<Chapter> SelectChapterByCode(int CodeChapter)
        {
            var Chapters = new Chapter();
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(selectChaptersByCode, sqlConnection);
            cmd.Parameters.AddWithValue("@aCodeChapter", CodeChapter);
            await sqlConnection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            Chapters = (ChapterDataReader(reader));
            return Chapters;
        }
        public static Chapter ChapterDataReader(SqlDataReader reader)
        {
            return new Chapter
            {
                CodeChapter = (int)reader["CodeChapter"],
                Name = reader["Name"].ToString(),
            };
        }
    }
}