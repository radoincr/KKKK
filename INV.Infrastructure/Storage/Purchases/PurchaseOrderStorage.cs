using System.Data;
using INV.App.Purchases;
using INV.Domain.Entities.Budget;
using INV.Domain.Entities.Purchases;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.Purchases;

public class PurchaseOrderStorage : IPurchaseOrderStorage
{
    private const string selectAllPurchaseOrderByDateQuery =
        " SELECT * FROM [purchase].[ORDERS] WHERE CAST(Date AS DATE) = @aSelectedDate";

    private const string selectPurchaseOrdersInfoQuery = @"SELECT
        p.[Number],p.[Id],p.[SupplierId],s.CompanyName As CompanyName,p.[Date],p.[Status] FROM
        [purchase].[ORDERS] p left Join SUPPLIERS s ON p.SupplierId=s.Id  ";

    private const string selectAllPurchaseOrderByIdSupplierQuery =
        "SELECT * FROM purchase.GetListBySupplier(@aSupplierId)";

    private const string selectPurchaseProductsQuery = @" SELECT * FROM [purchase].[PRODUCTS]";

    private const string selectPurchceOrderByIdQuery = @" SELECT * FROM [INV].[purchase].[ORDERS] WHERE Id=@aId";

    private const string insertOrderDetailCommand = @"
            INSERT INTO [purchase].[PRODUCTS] (PurchaseId, ProductId, Quantity, UnitPrice)
            VALUES (@aPurchaseId, @aProductId, @aQuantity, @aUnitPrice)";

    private const string insertPurchaseOrderCommand = @"
            INSERT INTO [purchase].[ORDERS] (Id, Number, SupplierId, Date, BudgetArticle, BudgetType,
                                       ServiceType, TotalHT, TotalVA, TotalTC, CompletionDelay)
            VALUES (@aId, @aNumber, @aSupplierId, @aDate, @aBudgetArticle, @aBudgetType,
                    @aServiceType, @aTotalHT, @aTotalTVA, @aTotalTTC, @aCompletionDelay)";

    private const string validatePurchaseCommand =
        @" UPDATE purchase.ORDERS SET VisaNumber=@aVisaNumber , VisaDate=@aVisaDate ,Status=@aStatus Where Id=@aId";

    private const string SelectPurchasesForReceiptCreationCommand = "reception.SelectPurchasesForReceiptCreation";
    private readonly string _connectionString;

    public PurchaseOrderStorage(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("INV");
    }

    public async IAsyncEnumerable<PurchaseOrderInfo> SelectPurchaseOrderInfo()
    {
        await using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectPurchaseOrdersInfoQuery, sqlConnection);
        await sqlConnection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync()) yield return getPurchaseOrdersInfoData(reader);
    }

    public async Task<int> InsertPurchaseOrder(PurchaseOrder purchaseOrder)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(insertPurchaseOrderCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aId", purchaseOrder.Id);
        cmd.Parameters.AddWithValue("@aNumber", /*purchaseOrder.Number*/"1");
        cmd.Parameters.AddWithValue("@aSupplierId", purchaseOrder.SupplierId);
        cmd.Parameters.AddWithValue("@aDate", purchaseOrder.Date);
        cmd.Parameters.AddWithValue("@aBudgetArticle", purchaseOrder.BudgeArticle);
        cmd.Parameters.AddWithValue("@aBudgetType", purchaseOrder.BudgeType);
        cmd.Parameters.AddWithValue("@aServiceType", purchaseOrder.ServiceType);
        cmd.Parameters.AddWithValue("@aTotalHT", purchaseOrder.TotalHT);
        cmd.Parameters.AddWithValue("@aTotalTVA", purchaseOrder.TotalTVA);
        cmd.Parameters.AddWithValue("@aTotalTTC", purchaseOrder.TotalTTC);
        cmd.Parameters.AddWithValue("@aCompletionDelay", purchaseOrder.CompletionDelay);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<PurchaseOrder>> SelectPurchaseOrdersByDate(DateOnly selectedDate)
    {
        var purchaseOrders = new List<PurchaseOrder>();

        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectAllPurchaseOrderByDateQuery, sqlConnection);

        cmd.Parameters.AddWithValue("@aSelectedDate", selectedDate);

        await sqlConnection.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var purchaseOrder = getPurchaseOrdersData(reader);
            purchaseOrders.Add(purchaseOrder);
        }

        return purchaseOrders;
    }

    public async Task<PurchaseOrder?> SelectPurchaseOrdersByID(Guid id)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        using var cmd = new SqlCommand(selectPurchceOrderByIdQuery, sqlConnection);
        cmd.Parameters.AddWithValue("@aId", id);

        using var reader = await cmd.ExecuteReaderAsync();

        return await reader.ReadAsync() ? getPurchaseOrdersData(reader) : null;
    }

    public async Task<int> InsertPurchaseProduct(PurchaseProduct orderDetail)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(insertOrderDetailCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aPurchaseOrderId", orderDetail.PurchaseOrderId);
        cmd.Parameters.AddWithValue("@aProductId", orderDetail.ProductId);
        cmd.Parameters.AddWithValue("@aQuantity", orderDetail.Quantity);
        cmd.Parameters.AddWithValue("@aUnitPrice", orderDetail.UnitPrice);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<PurchaseProduct>> SelectAllPurchaseProduct()
    {
        var orderDetails = new List<PurchaseProduct>();

        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectPurchaseProductsQuery, sqlConnection);
        await sqlConnection.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) orderDetails.Add(getPurchaseProductsData(reader));

        return orderDetails;
    }

    public async Task<List<PurchaseOrderInfo>> SelectPurchaseOrdersByIdSupplier(Guid supplierId)
    {
        var purchaseOrders = new List<PurchaseOrderInfo>();

        await using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectAllPurchaseOrderByIdSupplierQuery, sqlConnection);

        cmd.Parameters.AddWithValue("@aSupplierId", supplierId);

        await sqlConnection.OpenAsync();
        var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var purchaseOrder = getPurchaseOrdersInfoData(reader);
            purchaseOrders.Add(purchaseOrder);
        }

        return purchaseOrders;
    }

    public async Task<int> ValidatePurchase(PurchaseOrder purchaseOrder)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(validatePurchaseCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aId", purchaseOrder.Id);
        cmd.Parameters.AddWithValue("@aVisaNumber", purchaseOrder.VisaNumber);
        cmd.Parameters.AddWithValue("@aVisaDate", purchaseOrder.VisaDate);
        cmd.Parameters.AddWithValue("@aStatus", PurchaseStatus.Validated.ToString());
        return await cmd.ExecuteNonQueryAsync();
    }

    public async ValueTask<List<PurchaseOrderInfo>> SelectPurchasesForReceiptCreation()
    {
        var purchaseOrdersInfo = new List<PurchaseOrderInfo>();
        var purchaseProducts = new List<PurchaseProduct>();

        using var sqlConnection = new SqlConnection(_connectionString);
        await sqlConnection.OpenAsync();

        using var cmd = new SqlCommand(SelectPurchasesForReceiptCreationCommand, sqlConnection)
        {
            CommandType = CommandType.StoredProcedure
        };

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync()) purchaseOrdersInfo.Add(getPurchaseOrdersInfoData(reader));


        return purchaseOrdersInfo;
    }

    private static PurchaseOrder getPurchaseOrdersData(SqlDataReader reader)
    {
        return new PurchaseOrder
        {
            Id = (Guid)reader["Id"],
            Number = (string)reader["Number"],
            SupplierId = (Guid)reader["SupplierId"],
            Date = DateOnly.FromDateTime((DateTime)reader["Date"]),
            BudgeArticle = (string)reader["BudgetArticle"],
            BudgeType = (BudgeType)reader["BudgetType"],
            ServiceType = (ServiceType)reader["ServiceType"],
            TotalHT = (decimal)reader["TotalHT"],
            TotalTVA = (decimal)reader["TotalVA"],
            TotalTTC = (decimal)reader["TotalTC"],
            CompletionDelay = (int)reader["CompletionDelay"],
            VisaNumber = reader.IsDBNull(reader.GetOrdinal("VisaNumber")) ? null : reader["VisaNumber"].ToString(),
            VisaDate = reader.IsDBNull(reader.GetOrdinal("VisaDate"))
                ? null
                : DateOnly.FromDateTime((DateTime)reader["VisaDate"]),
            Status = (PurchaseStatus)reader["Status"]
        };
    }

    private static PurchaseOrderInfo getPurchaseOrdersInfoData(SqlDataReader reader)
    {
        var r = new PurchaseOrderInfo
        {
            Id = (Guid)reader["Id"],
            SupplierId = (Guid)reader["SupplierId"],
            Number = (string)reader["Number"],
            Status = (PurchaseStatus)reader["Status"],
            SupplierName = (string)reader["CompanyName"],
            Date = DateOnly.FromDateTime((DateTime)reader["Date"])
        };
        return r;
    }

    private static PurchaseProduct getPurchaseProductsData(SqlDataReader reader)
    {
        return new PurchaseProduct
        {
            ProductId = (Guid)reader["ProductId"],
            PurchaseOrderId = (Guid)reader["Id"],
            Quantity = (int)reader["Quantity"],
            UnitPrice = (decimal)reader["UnitPrice"]
        };
    }
}