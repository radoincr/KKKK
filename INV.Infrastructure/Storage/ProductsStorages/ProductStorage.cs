using INV.Domain.Entities.ProductEntity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INV.Infrastructure.Storage.ProductsStorages
{
    public class ProductStorage : IProductStorage
    {
        private readonly string _connectionString;

        public ProductStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }

        private const string insertProduct = @"
            INSERT INTO Product (ID, IDPurchaseOrder, Designation, UnitMeasure, DefaultTVARate, Quantity, UnitPrice, TVA,TotalPrice) 
            VALUES (@ID, @IDPurchaseOrder, @Designation, @UnitMeasure, @DefaultTVARate, @Quantity, @UnitPrice, @TVA,@TotalPrice)";

        private const string updateProduct = @"
            UPDATE Product 
            SET IDPurchaseOrder = @IDPurchaseOrder,
                Designation = @Designation,
                UnitMeasure = @UnitMeasure,
                DefaultTVARate = @DefaultTVARate,
                Quantity = @Quantity,
                UnitPrice = @UnitPrice,
                TVA = @TVA,
                TotalPrice = @TotalPrice
            WHERE ID = @ID";

        private const string deleteProductQuery = @"Delete from Product where ID=@ID";

        private const string selectProductByIdPurchaseOrder = @"
            SELECT ID, IDPurchaseOrder, Designation, UnitMeasure, DefaultTVARate, Quantity, UnitPrice, TVA ,TotalPrice
            FROM Product 
            WHERE IDPurchaseOrder = @IDPurchaseOrder";

        private static Product getProductFromReader(SqlDataReader reader)
        {
            return new Product
            {
                ID = (Guid)reader["ID"],
                IDPurchaseOrder = (Guid)reader["IDPurchaseOrder"],
                Designation = reader["Designation"].ToString(),
                UnitMeasure = reader["UnitMeasure"].ToString(),
                DefaultTVARate = (int)reader["DefaultTVARate"],
                Quantity = Convert.ToInt32(reader["Quantity"]),
                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                TVA = Convert.ToDecimal(reader["TVA"]),
                TotalePrice = Convert.ToDecimal(reader["TotalPrice"])
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
                cmd.Parameters.AddWithValue("@IDPurchaseOrder", product.IDPurchaseOrder);
                cmd.Parameters.AddWithValue("@Designation", product.Designation);
                cmd.Parameters.AddWithValue("@UnitMeasure", product.UnitMeasure);
                cmd.Parameters.AddWithValue("@DefaultTVARate", product.DefaultTVARate);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                cmd.Parameters.AddWithValue("@TVA", product.TVA);
                cmd.Parameters.AddWithValue("@TotalPrice", product.TotalePrice);
                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateProduct(Product product)
        {
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(updateProduct, sqlConnection);
                await sqlConnection.OpenAsync();

                cmd.Parameters.AddWithValue("@ID", product.ID);
                cmd.Parameters.AddWithValue("@IDPurchaseOrder", product.IDPurchaseOrder);
                cmd.Parameters.AddWithValue("@Designation", product.Designation);
                cmd.Parameters.AddWithValue("@UnitMeasure", product.UnitMeasure);
                cmd.Parameters.AddWithValue("@DefaultTVARate", product.DefaultTVARate);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                cmd.Parameters.AddWithValue("@TVA", product.TVA);
                cmd.Parameters.AddWithValue("@TotalPrice", product.TotalePrice);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteProduct(Guid id)
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(deleteProductQuery, sqlConnection);
            await sqlConnection.OpenAsync();
            cmd.Parameters.AddWithValue("@ID", id);
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Product>> SelectProductsByPurchaseOrderId(Guid purchaseOrderId)
        {
            var products = new List<Product>();
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(selectProductByIdPurchaseOrder, sqlConnection);
                cmd.Parameters.AddWithValue("@IDPurchaseOrder", purchaseOrderId);
                await sqlConnection.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var product = getProductFromReader(reader);
                    products.Add(product);
                }

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}