﻿using System.Data;
using INV.App.Receipts;
using INV.Domain.Entities.Receipts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.Receipts;

public partial class ReceiptStorage : IReceiptStorage
{
    private const string selectAllReceiptsQuery = "SELECT * FROM [reception].[List]";
    private const string selectReceiptByIdQuery = "SELECT * FROM [reception].[HEADERS] WHERE Id = @aId";

    private const string selectReceiptsByPurchaseIdQuery =
        "SELECT * FROM [reception].[HEADERS] WHERE PurchaseId = @aPurchaseId";

    private const string insertReceiptCommand = @"
            INSERT INTO [reception].[HEADERS] (Id, PurchaseId, Date, DeliveryNumber, DeliveryDate, Status)
            VALUES (@aId, @aPurchaseId, @aDate, @aDeliveryNumber, @aDeliveryDate, @aStatus)";

    private const string updateReceiptCommand = @"
            UPDATE [reception].[HEADERS]
            SET PurchaseId = @aPurchaseId, Date = @aDate, DeliveryNumber = @aDeliveryNumber,
                DeliveryDate = @aDeliveryDate, Status = @aStatus
            WHERE Id = @aId";

    private const string deleteReceiptCommand = "DELETE FROM [reception].[HEADERS] WHERE Id = @aId";

    // SQL Queries for ReceiptProduct
    private const string selectAllReceiptProductsQuery = "SELECT * FROM [reception].[PRODUCTS]";

    private const string selectProductsByReceptionIdQuery =
        "SELECT * FROM [reception].[PRODUCTS] WHERE ReceptionId = @aReceptionId";

    private const string insertReceiptProductCommand = @"
            INSERT INTO [reception].[PRODUCTS] (ReceptionId, ProductId, Quantity)
            VALUES (@aReceptionId, @aProductId, @aQuantity)";

    private const string updateReceiptProductCommand = @"
            UPDATE [reception].[PRODUCTS]
            SET Quantity = @aQuantity
            WHERE ReceptionId = @aReceptionId AND ProductId = @aProductId";

    private const string deleteReceiptProductCommand = @"
            DELETE FROM [reception].[PRODUCTS] WHERE ReceptionId = @aReceptionId AND ProductId = @aProductId";

    private const string getReceptionDetailsProcedure = "dbo.GetReceptionDetails";
    private const string CreateReceiptFromPurchaseCommand = "reception.CreateFromPurchase";
    private const string getReceiptInfoById = "[reception].[GetById]";
    private readonly string _connectionString;

    public ReceiptStorage(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("INV");
    }

    public async ValueTask<ReceiptInfo> CreateReceiptFromPurchase(Guid purchaseId)
    {
        using var connection = new SqlConnection(_connectionString);

        using var cmd = new SqlCommand(CreateReceiptFromPurchaseCommand, connection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@aPurchaseId", purchaseId);
        DataSet ds = new();
        SqlDataAdapter da = new(cmd);
        await connection.OpenAsync();
        da.Fill(ds);
        return getReceiptFromDataSet(ds, true);
    }

    public async ValueTask<List<ReceiptInfo>> SelectAllReceipts()
    {
        await using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectAllReceiptsQuery, sqlConnection);
        await sqlConnection.OpenAsync();

        var reader = await cmd.ExecuteReaderAsync();
        var receipts = new List<ReceiptInfo>();
        while (await reader.ReadAsync()) receipts.Add(getReceiptInfoFromReader(reader));

        return receipts;
    }

    public async ValueTask<Receipt?> SelectReceiptById(Guid id)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectReceiptByIdQuery, sqlConnection);
        cmd.Parameters.AddWithValue("@aId", id);
        await sqlConnection.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync() ? GetReceiptData(reader) : null;
    }

    public async ValueTask<List<Receipt>> SelectReceiptsByPurchaseId(Guid purchaseId)
    {
        var receipts = new List<Receipt>();
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectReceiptsByPurchaseIdQuery, sqlConnection);
        cmd.Parameters.AddWithValue("@aPurchaseId", purchaseId);
        await sqlConnection.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) receipts.Add(GetReceiptData(reader));

        return receipts;
    }

    public async ValueTask<int> InsertReceipt(Receipt receipt)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(insertReceiptCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aId", receipt.Id);
        cmd.Parameters.AddWithValue("@aPurchaseId", receipt.PurchaseId);
        cmd.Parameters.AddWithValue("@aDate", receipt.Date);
        cmd.Parameters.AddWithValue("@aDeliveryNumber", receipt.DeliveryNumber);
        cmd.Parameters.AddWithValue("@aDeliveryDate", receipt.DeliveryDate);
        cmd.Parameters.AddWithValue("@aStatus", receipt.Status);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async ValueTask<int> UpdateReceipt(Receipt receipt)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(updateReceiptCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aId", receipt.Id);
        cmd.Parameters.AddWithValue("@aPurchaseId", receipt.PurchaseId);
        cmd.Parameters.AddWithValue("@aDate", receipt.Date);
        cmd.Parameters.AddWithValue("@aDeliveryNumber", receipt.DeliveryNumber);
        cmd.Parameters.AddWithValue("@aDeliveryDate", receipt.DeliveryDate);
        cmd.Parameters.AddWithValue("@aStatus", receipt.Status);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async ValueTask<int> DeleteReceipt(Guid id)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(deleteReceiptCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aId", id);
        return await cmd.ExecuteNonQueryAsync();
    }

    public async ValueTask<List<ReceiptProduct>> SelectAllReceiptProducts()
    {
        var receiptProducts = new List<ReceiptProduct>();
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectAllReceiptProductsQuery, sqlConnection);
        await sqlConnection.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) receiptProducts.Add(GetReceiptProductData(reader));

        return receiptProducts;
    }

    public async ValueTask<List<ReceiptProduct>> SelectProductsByReceptionId(Guid receptionId)
    {
        var receiptProducts = new List<ReceiptProduct>();
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(selectProductsByReceptionIdQuery, sqlConnection);
        cmd.Parameters.AddWithValue("@aReceptionId", receptionId);
        await sqlConnection.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) receiptProducts.Add(GetReceiptProductData(reader));

        return receiptProducts;
    }

    public async ValueTask<int> InsertReceiptProduct(ReceiptProduct receiptProduct)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(insertReceiptProductCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aReceptionId", receiptProduct.ReceptionId);
        cmd.Parameters.AddWithValue("@aProductId", receiptProduct.ProductId);
        cmd.Parameters.AddWithValue("@aQuantity", receiptProduct.Quantity);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async ValueTask<int> UpdateReceiptProduct(ReceiptProduct receiptProduct)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(updateReceiptProductCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aReceptionId", receiptProduct.ReceptionId);
        cmd.Parameters.AddWithValue("@aProductId", receiptProduct.ProductId);
        cmd.Parameters.AddWithValue("@aQuantity", receiptProduct.Quantity);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async ValueTask<int> DeleteReceiptProduct(Guid receptionId, Guid productId)
    {
        using var sqlConnection = new SqlConnection(_connectionString);
        var cmd = new SqlCommand(deleteReceiptProductCommand, sqlConnection);
        await sqlConnection.OpenAsync();

        cmd.Parameters.AddWithValue("@aReceptionId", receptionId);
        cmd.Parameters.AddWithValue("@aProductId", productId);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async ValueTask<ReceiptInfo> GetReceiptInfoById(Guid receiptId, bool includeProducts = false)
    {
        await using var sqlConnection = new SqlConnection(_connectionString);

        var cmd = new SqlCommand(getReceiptInfoById, sqlConnection)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@aReceptionId", receiptId);
        var ds = new DataSet();
        var da = new SqlDataAdapter(cmd);
        await sqlConnection.OpenAsync();
        da.Fill(ds);

        return getReceiptFromDataSet(ds, includeProducts);
    }

    public async ValueTask ValidateReceipt(Guid receiptId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("[reception].[Validate]", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@aReceiptId", receiptId);

            var returnValue = new SqlParameter
            {
                ParameterName = "@RETURN_VALUE",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(returnValue);

            await cmd.ExecuteNonQueryAsync();

            var result = (int)returnValue.Value;

            switch (result)
            {
                case 2001:
                    throw new InvalidOperationException("Cannot validate: receipt deja validee");
                case 2002:
                    throw new InvalidOperationException("Cannot validate: rest a livrer <received");
            }
        }
        catch (SqlException ex)
        {
            throw new Exception($"Database error occurred while validating receipt: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error validating receipt: {ex.Message}", ex);
        }
    }

    private ReceiptInfo getReceiptFromDataSet(DataSet ds, bool includeProducts)
    {
        ReceiptInfo receiptInfo = null;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            receiptInfo = GetReceiptInfoFromDataRow(ds.Tables[0].Rows[0]);
            receiptInfo.ReceiptProducts = new List<ReceiptProductInfo>();
            if (includeProducts)
                if (ds.Tables.Count > 1)
                    foreach (DataRow productRow in ds.Tables[1].Rows)
                        receiptInfo.ReceiptProducts.Add(GetReceiptProductInfoDataFromDataRow(productRow));
        }

        return receiptInfo;
    }
}