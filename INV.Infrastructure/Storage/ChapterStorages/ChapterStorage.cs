using INV.Domain.Entity.ChapterEntity;
using INV.Domain.Entity.ChapterEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using INV.Domain.Entity.SupplierEntity;
namespace INV.Infrastructure.Storage.ChapterStorages
{
    public class ChapterStorage : IChapterStorage
    {
        private readonly string _connectionString;
        public ChapterStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }
        private const string insertChapterQuery = "INSERT INTO Chapter (CodeChapter,Name) VALUES (@CodeChapter,@Name)";
        private const string selectAll = "SELECT * FROM Chapter";
        private const string selectChaptersByCode = "Select * From Chapter where CodeChapter=@CodeChapter ";
        public async Task<int> InsertChapter(Chapter Chapter)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(insertChapterQuery, sqlConnection);
            cmd.Parameters.AddWithValue("@CodeChapter", Chapter.CodeChapter);
            cmd.Parameters.AddWithValue("@Name", Chapter.Name);
            await sqlConnection.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }
        public async Task<List<Chapter>> SelectAll()
        {
            var chapters = new List<Chapter>();
            using var sqlConnection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(selectAll, sqlConnection);
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
            cmd.Parameters.AddWithValue("@CodeChapter", CodeChapter);
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