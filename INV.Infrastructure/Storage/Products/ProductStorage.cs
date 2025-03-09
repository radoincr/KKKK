using INV.Domain.Entities.Products;
using INV.Domain.Entities.Purchases;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.Products;

public class ProductStorage : IProductStorage
{
    private const string insertProductCommand = @"
            INSERT INTO [dbo].[PRODUCTS] ( Id, Designation,UnitMeasure,Quantity, UnitPrice, TVA)
            VALUES (@aId, @aDesignation,@aUnitMeasure, @aQuantity, @aUnitPrice, @aTVA)";

    private const string insertProductPurchaseCommand = @"
            INSERT INTO [purchase].[PRODUCTS] ( PurchaseId, ProductId,Quantity,UnitPrice)
            VALUES (@aPurchaseId, @aProductId,@aQuantity, @aUnitPrice)";

    private const string updateProductCommand = @"
            UPDATE [dbo].[PRODUCTS] SET Designation = @aDesignation, UnitMeasure = @aUnitMeasure,Quantity = @aQuantity,
            UnitPrice = @aUnitPrice, TVA = @aTVA  WHERE Id = @aId";

    private const string deleteProductCommand = @"
            Delete from [dbo].[PRODUCTS] where Id=@aId";

    private const string selectProductsQuery = @"
            SELECT * FROM [dbo].[PRODUCTS] ";

    private const string selectProductCountByIdQuery = @"
            SELECT count(*) FROM Products WHERE Designation = @aDesignation";

    private readonly string _connectionString;

    public ProductStorage(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("INV");
    }

    public async Task<int> InsertProduct(Product product)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(insertProductCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aId", product.Id);
        cmd.Parameters.AddWithValue("@aDesignation", product.Designation);
        cmd.Parameters.AddWithValue("@aUnitMeasure", product.UnitMeasure);
        cmd.Parameters.AddWithValue("@aQuantity", product.Quantity);
        cmd.Parameters.AddWithValue("@aUnitPrice", product.UnitPrice);
        cmd.Parameters.AddWithValue("@aTVA", product.TVA);
        return await cmd.ExecuteNonQueryAsync();
    }

    public async ValueTask InsertProductPurchase(PurchaseProduct purchaseProduct)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(insertProductPurchaseCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aPurchaseId", purchaseProduct.PurchaseOrderId);
        cmd.Parameters.AddWithValue("@aProductId", purchaseProduct.ProductId);
        cmd.Parameters.AddWithValue("@aQuantity", purchaseProduct.Quantity);
        cmd.Parameters.AddWithValue("@aUnitPrice", purchaseProduct.UnitPrice);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<int> UpdateProduct(Product product)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(updateProductCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aId", product.Id);
        cmd.Parameters.AddWithValue("@aDesignation", product.Designation);
        cmd.Parameters.AddWithValue("@aUnitMeasure", product.UnitMeasure);
        cmd.Parameters.AddWithValue("@aQuantity", product.Quantity);
        cmd.Parameters.AddWithValue("@aUnitPrice", product.UnitPrice);
        cmd.Parameters.AddWithValue("@aTVA", product.TVA);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<int> DeleteProduct(Guid id)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(deleteProductCommand, sqlConnection);
        await sqlConnection.OpenAsync();
        cmd.Parameters.AddWithValue("@aId", id);
        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<Product>> SelectProducts()
    {
        var products = new List<Product>();

        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectProductsQuery, sqlConnection);
        await sqlConnection.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var product = getProductData(reader);
            products.Add(product);
        }

        return products;
    }

    public async Task<bool> ProductExistsByaDesignation(string designation)
    {
        using var connection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectProductCountByIdQuery, connection);
        cmd.Parameters.AddWithValue("@aDesignation", designation);
        connection.Open();

        var count = (int)(await cmd.ExecuteScalarAsync() ?? 0);
        return count > 0;
    }

    private static Product getProductData(SqlDataReader reader)
    {
        return new Product
        {
            Id = (Guid)reader["Id"],
            Designation = (string)reader["Designation"],
            UnitMeasure = (string)reader["UnitMeasure"],
            Quantity = (int)reader["Quantity"],
            UnitPrice = (decimal)reader["UnitPrice"],
            TVA = (int)reader["TVA"]
        };
    }
}