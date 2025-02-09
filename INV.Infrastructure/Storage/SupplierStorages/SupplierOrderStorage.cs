using INV.Domain.Entity.PurchaseOrderEntity;
using INV.Domain.Entity.SupplierEntity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.SupplierStorages;

public class SupplierOrderStorage : ISupplierOrderStorage
{
    private readonly string _connectionString;

    public SupplierOrderStorage(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("INV");
    }
    private const string selectAllPurchaseOrderByIDSupplier = @"SELECT * FROM [INV].[dbo].[PurchaseOrder]
           where IDSupplier=@IDSupplier"; 
    private static PurchaseOrder getPurchaseOrders(SqlDataReader reader)
    {
        return new PurchaseOrder
        {
            ID = (Guid)reader["ID"],
            IDSupplier = (Guid)reader["IDSupplier"],
            Number = (int)reader["Number"],
            Date = DateOnly.FromDateTime((DateTime)reader["Date"]),
            Status = reader["State"].ToString(),
            Chapter = reader["Chapter"].ToString(),
            Article = reader["Article"].ToString(),
            TypeBudget = reader["TypeBudget"].ToString(),
            TypeService = reader["TypeService"].ToString(),
            THT = (decimal)reader["THT"],
            TVA = (decimal)reader["TVA"],
            TTC = (decimal)reader["TTC"],
            CompletionDelay = (int)reader["CompletionDelay"]
        };
    }
    
    public async Task<List<PurchaseOrder>> SelectPurchaseOrdersByIDSupplier(Guid IDSupplier)
    {
        var purchaseOrders = new List<PurchaseOrder>();
        try
        {
            using var sqlConnection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(selectAllPurchaseOrderByIDSupplier, sqlConnection);

            cmd.Parameters.AddWithValue("@IDSupplier", IDSupplier);

            await sqlConnection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var purchaseOrder = getPurchaseOrders(reader);
                purchaseOrders.Add(purchaseOrder);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return purchaseOrders;
    }

}