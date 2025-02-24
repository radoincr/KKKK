using System.Data;
using INV.App.Receipts;
using INV.Domain.Entities.Budget;
using INV.Domain.Entities.Purchases;
using INV.Domain.Entities.Receipts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.Receipts
{
    public class ReceiptStorage : IReceiptStorage
    {
        private readonly string _connectionString;

        public ReceiptStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }
        private const string selectAllReceiptsQuery = "SELECT * FROM [reception].[HEADERS]";
        private const string selectReceiptByIdQuery = "SELECT * FROM [reception].[HEADERS] WHERE Id = @aId";
        private const string selectReceiptsByPurchaseIdQuery = "SELECT * FROM [reception].[HEADERS] WHERE PurchaseId = @aPurchaseId";
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
        private const string selectProductsByReceptionIdQuery = "SELECT * FROM [reception].[PRODUCTS] WHERE ReceptionId = @aReceptionId";
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

        private static Receipt GetReceiptData(SqlDataReader reader)
        {
            return new Receipt
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                PurchaseId = reader.GetGuid(reader.GetOrdinal("PurchaseId")),
                Date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("Date"))),
                DeliveryNumber = reader.GetString(reader.GetOrdinal("DeliveryNumber")),
                DeliveryDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DeliveryDate"))),
                Status = (ReceiptStatus)reader.GetInt32(reader.GetOrdinal("Status"))
            };
        }
        private static ReceiptInfo GetReceiptInfo(SqlDataReader reader)
        {
            return new ReceiptInfo()
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                PurchaseId = reader.GetGuid(reader.GetOrdinal("PurchaseId")),
                Date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("Date"))),
                purchaseNumber = reader.GetString(reader.GetOrdinal("purchaseNum")),
                supplierId = reader.GetGuid(reader.GetOrdinal("supplierId")),
                supplierName = reader.GetString(reader.GetOrdinal("supplierName")),
                DeliveryNumber = reader.GetString(reader.GetOrdinal("DeliveryNumber")),
                DeliveryDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DeliveryDate"))),
                Status = (ReceiptStatus)reader.GetInt32(reader.GetOrdinal("Status"))
            };
        }

        
        
        private static PurchaseOrder GetPurchaseOrderData(SqlDataReader reader)
        {
            return new PurchaseOrder
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Number = reader.GetInt32(reader.GetOrdinal("Number")),
                SupplierId = reader.GetGuid(reader.GetOrdinal("SupplierId")),
                Date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("Date"))),
                BudgeArticle = reader.GetString(reader.GetOrdinal("BudgetArticle")),
                BudgeType = (BudgeType)reader.GetInt32(reader.GetOrdinal("BudgetType")),
                ServiceType = (ServiceType)reader.GetInt32(reader.GetOrdinal("ServiceType")),
                TotalHT = reader.GetDecimal(reader.GetOrdinal("TotalHT")),
                TotalTVA = reader.GetDecimal(reader.GetOrdinal("TotalVA")),
                TotalTTC = reader.GetDecimal(reader.GetOrdinal("TotalTC")),
                CompletionDelay = reader.GetInt32(reader.GetOrdinal("CompletionDelay")),
                VisaNumber = reader.IsDBNull(reader.GetOrdinal("VisaNumber")) ? null : reader.GetString(reader.GetOrdinal("VisaNumber")),
                VisaDate = reader.IsDBNull(reader.GetOrdinal("VisaDate")) ? null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("VisaDate"))),
                Status = (PurchaseStatus)reader.GetInt32(reader.GetOrdinal("Status"))
            };
        }
        // Data mapping for ReceiptProduct
        private static ReceiptProduct GetReceiptProductData(SqlDataReader reader)
        {
            return new ReceiptProduct
            {
                ReceptionId = reader.GetGuid(reader.GetOrdinal("ReceptionId")),
                ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
            };
        }
        public async ValueTask<Guid> CreateReceiptFromPurchase(Guid purchaseId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                using var cmd = new SqlCommand(CreateReceiptFromPurchaseCommand, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@aPurchaseId", purchaseId);
                var receiptIdParameter = new SqlParameter("@aReceptionId", SqlDbType.UniqueIdentifier)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(receiptIdParameter);

                await cmd.ExecuteNonQueryAsync();

                return (Guid)receiptIdParameter.Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }


      

        public ValueTask ValidateReceipt(Guid receiptId)
        {
            throw new NotImplementedException();
        }
        
        public async ValueTask<List<Receipt>> SelectAllReceipts()
        {
            var receipts = new List<Receipt>();
            using var sqlConnection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(selectAllReceiptsQuery, sqlConnection);
            await sqlConnection.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                receipts.Add(GetReceiptData(reader));
            }

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
            while (await reader.ReadAsync())
            {
                receipts.Add(GetReceiptData(reader));
            }

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

        // ReceiptProduct CRUD Operations
        public async ValueTask<List<ReceiptProduct>> SelectAllReceiptProducts()
        {
            var receiptProducts = new List<ReceiptProduct>();
            using var sqlConnection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(selectAllReceiptProductsQuery, sqlConnection);
            await sqlConnection.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                receiptProducts.Add(GetReceiptProductData(reader));
            }

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
            while (await reader.ReadAsync())
            {
                receiptProducts.Add(GetReceiptProductData(reader));
            }

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
        
    
        public async ValueTask<(List<ReceiptProduct> products, Receipt? receipt, PurchaseOrder? purchaseOrder)> GetReceptionDetails(Guid receptionId)
        {
            var products = new List<ReceiptProduct>();
            Receipt? receipt = null;
            PurchaseOrder? purchaseOrder = null;

            using var sqlConnection = new SqlConnection(_connectionString);

            using var cmd = new SqlCommand(getReceptionDetailsProcedure, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@ReceptionId", receptionId);
            await sqlConnection.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(GetReceiptProductData(reader));
            }

            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                receipt = GetReceiptData(reader);
            }

            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                purchaseOrder = GetPurchaseOrderData(reader);
            }

            return (products, receipt, purchaseOrder);
        }

        public async ValueTask<ReceiptInfo> GetReceiptInfoById(Guid receiptId)
        {
            ReceiptInfo receiptInfo = null;

            await using var sqlConnection = new SqlConnection(_connectionString);
    
            var cmd = new SqlCommand(getReceiptInfoById, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@aReceptionId", receiptId);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            await sqlConnection.OpenAsync();
            da.Fill(ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var headerRow = ds.Tables[0].Rows[0];
                receiptInfo = new ReceiptInfo
                {
                    Id = (Guid)headerRow["Id"],
                    PurchaseId = (Guid)headerRow["PurchaseId"],
                    Date =  headerRow.IsNull("DeliveryDate") ? (DateOnly?)null : DateOnly.FromDateTime((DateTime)headerRow["Date"]),
                    purchaseNumber = headerRow.IsNull("purchaseNum") ? null : (string)headerRow["purchaseNum"],
                    supplierId = (Guid)headerRow["supplierId"],
                    supplierName = headerRow.IsNull("supplierName") ? null : (string)headerRow["supplierName"],
                    DeliveryNumber = headerRow.IsNull("DeliveryNumber") ? null : (string)headerRow["DeliveryNumber"],
                    DeliveryDate = headerRow.IsNull("DeliveryDate") ? (DateOnly?)null : DateOnly.FromDateTime((DateTime)headerRow["DeliveryDate"]),
                    Status = (ReceiptStatus)headerRow["Status"],
                    ReceiptProducts = new List<ReceiptProduct>()
                };

                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow productRow in ds.Tables[1].Rows)
                    {
                        receiptInfo.ReceiptProducts.Add(new ReceiptProduct
                        {
                            ReceptionId = (Guid)productRow["ReceptionId"],
                            ProductId = (Guid)productRow["ProductId"],
                            Quantity = (int)productRow["Quantity"]
                        });
                    }
                }
            }

            return receiptInfo;
        } 


    }
}