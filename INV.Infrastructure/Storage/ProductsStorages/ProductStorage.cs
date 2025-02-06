using Entity.ProductEntity;
using Intefrace.ProductStorages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Storage.ProductStorage
{
    public class ProductStorage : IProductStorage
    {
        private readonly string _connectionString;

        public ProductStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }

        private const string insertProduct = @"
            INSERT INTO Product (ID, Designation, UnitMeasure, DefaultTVARate) 
            VALUES (@ID, @Designation, @UnitMeasure, @DefaultTVARate)";

        private const string selectAllProducts = @"
            SELECT ID, Designation, UnitMeasure, DefaultTVARate 
            FROM Product";

        private static Product getProducts(SqlDataReader reader)
        {
            return new Product
            {
                ID = (Guid)reader["ID"],
                Designation = reader["Designation"].ToString(),
                UnitMeasure = reader["UnitMeasure"].ToString(),
                DefaultTVARate = reader["DefaultTVARate"].ToString()
            };
        }

      
        public async Task<int> InsertProduct(Product product)
        {
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(insertProduct, sqlConnection);
                await sqlConnection.OpenAsync();

                cmd.Parameters.AddWithValue("@ID", product.ID);
                cmd.Parameters.AddWithValue("@Designation", product.Designation);
                cmd.Parameters.AddWithValue("@UnitMeasure", product.UnitMeasure);
                cmd.Parameters.AddWithValue("@DefaultTVARate", product.DefaultTVARate);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }

    
        public async Task<List<Product>> SelectAllProduct()
        {
            var products = new List<Product>();
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(selectAllProducts, sqlConnection);
                await sqlConnection.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var product = getProducts(reader);
                    products.Add(product);
                }
                return products;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
