using System.Data;
using Entity.OrderDetailsEntity;
using Entity.ProductPDF;
using Entity.PurchaseOrderEntity;
using Entity.SupplierEntity;
using Interface.PurchaseOrderStorage;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Storage.PurchaseOrderStorage
{
    public class PurchaseOrderStorage : IPurchaseOrderStorage
    {
        private readonly string _connectionString;

        public PurchaseOrderStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }


        private const string insertPurchaseOrder = @"
            INSERT INTO PurchaseOrder (ID, IDSupplier, Date, State, Chapter, Article, 
                                       TypeBudget, TypeService, THT, TVA, TTC, CompletionDelay) 
            VALUES (@ID, @IDSupplier, @Date, @Status, @Chapter, @Article, 
                    @TypeBudget, @TypeService, @THT, @TVA, @TTC, @CompletionDelay)";


        private const string selectAllPurchaseOrders = @"
            SELECT *  FROM PurchaseOrder";


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


        public async Task<int> InsertPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(insertPurchaseOrder, sqlConnection);
                await sqlConnection.OpenAsync();

                cmd.Parameters.AddWithValue("@ID", purchaseOrder.ID);
                cmd.Parameters.AddWithValue("@IDSupplier", purchaseOrder.IDSupplier);
                cmd.Parameters.AddWithValue("@Date", purchaseOrder.Date.ToDateTime(TimeOnly.MinValue));
                cmd.Parameters.AddWithValue("@Status", purchaseOrder.Status);
                cmd.Parameters.AddWithValue("@Chapter", purchaseOrder.Chapter);
                cmd.Parameters.AddWithValue("@Article", purchaseOrder.Article);
                cmd.Parameters.AddWithValue("@TypeBudget", purchaseOrder.TypeBudget);
                cmd.Parameters.AddWithValue("@TypeService", purchaseOrder.TypeService);
                cmd.Parameters.AddWithValue("@THT", purchaseOrder.THT);
                cmd.Parameters.AddWithValue("@TVA", purchaseOrder.TVA);
                cmd.Parameters.AddWithValue("@TTC", purchaseOrder.TTC);
                cmd.Parameters.AddWithValue("@CompletionDelay", purchaseOrder.CompletionDelay);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task<List<PurchaseOrder>> SelectAllPurchaseOrder()
        {
            var purchaseOrders = new List<PurchaseOrder>();
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(selectAllPurchaseOrders, sqlConnection);
                await sqlConnection.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var purchaseOrder = getPurchaseOrders(reader);
                    purchaseOrders.Add(purchaseOrder);
                }

                return purchaseOrders;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<(List<PurchaseOrder>, List<Supplier>, List<ProductPdf>)> SelectPurchaseOrderDetails(
            int purchaseOrderNumber)
        {
            var purchaseOrders = new List<PurchaseOrder>();
            var suppliers = new List<Supplier>();
            var productPdf = new List<ProductPdf>();
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("SelectPurchaseOrderByNumber", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PurchaseOrderNumber", purchaseOrderNumber);
                await sqlConnection.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var purchaseOrder = new PurchaseOrder
                    {
                        Number = (int)reader["PurchaseOrderNumber"],
                        Date = DateOnly.FromDateTime((DateTime)reader["OrderDate"]) ,
                        Status = reader["OrderState"]?.ToString(),
                        TypeBudget = reader["TypeBudget"].ToString(),
                        TypeService = reader["TypeService"].ToString(),
                        Chapter = reader["Chapter"].ToString(),
                        Article = reader["Article"].ToString(),
                        THT = (decimal)reader["THT"],
                        TVA = (decimal)reader["TVA"],
                        TTC = (decimal)reader["TTC"],
                        CompletionDelay =(int)reader["CompletionDelay"]
                    };
                    purchaseOrders.Add(purchaseOrder);
                }
                await reader.NextResultAsync();
                while (await reader.ReadAsync())
                {
                    var supplier = new Supplier
                    {
                        SupplierName = reader["SupplierName"].ToString(),
                        CompanyName = reader["CompanyName"].ToString(),
                        AccountName = reader["AccountName"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        RC = reader["RC"].ToString(),
                        NIS = (int)reader["NIS"],
                        NIF = (long)reader["NIF"],
                        RIB = reader["RIB"].ToString(),
                        BankAgency = reader["BankAgency"].ToString()
                    };
                    suppliers.Add(supplier);
                }
                await reader.NextResultAsync();
                while (await reader.ReadAsync())
                {
                    var product_pdf = new ProductPdf
                    {
                        Designation =reader["Designation"].ToString() ,
                        Unitmesure = reader["UnitMeasure"].ToString(),
                        Quantity = (int)reader["Quantity"],
                        Price = (decimal)reader["UnitPrice"],
                        TVA = reader["OrderDetailTVA"].ToString(),
                    };
                    productPdf.Add(product_pdf);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }
            return (purchaseOrders, suppliers, productPdf);
        }
    }
}