using System.Data;
using INV.App.PurchaseOrders;
using INV.Domain.Entities.ProductPDF;
using INV.Domain.Entities.PurchaseOrders;
using INV.Domain.Entities.SupplierEntity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.PurchaseOrderStorages
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


        private const string selectAllPurchaseOrderByDate = 
            " SELECT * FROM [INV].[dbo].[PurchaseOrder] WHERE CAST(Date AS DATE) = @SelectedDate";
            

        private const string selectPurchaseOrdersInfo = @"SELECT
        p.[Number],p.[ID],p.[IDSupplier],s.SupplierName As SupplierName,p.[Date],p.[State] FROM
         [INV].[dbo].[PurchaseOrder] p left Join Supplier s ON p.IDSupplier=s.ID  ";

        private const string selectAllPurchaseOrderByIDSupplier = @"SELECT * FROM [INV].[dbo].[PurchaseOrder]
        where IDSupplier=@IDSupplier"; 
        
        private const string insertOrderDetail = @"
            INSERT INTO OrderDetail (ID, IDPurchaseOrder, IdProducts, Quantity, UnitPrice, TVA)
            VALUES (@ID, @IDPurchaseDetail, @IdProducts, @Quantity, @UnitPrice, @TVA)";
        
        private const string selectAllOrderDetails = @" SELECT * FROM OrderDetail";

        private const string selectPurchceOrderByIdQuery = @" SELECT * FROM [INV].[dbo].[PurchaseOrder] WHERE ID=@ID";

        private const string updatePurchaseOrderByIDQuery =
            @" UPDATE [INV].[dbo].[PurchaseOrder] SET [B]=@B , [FI]=@Fi ,State=@State Where ID=@ID";
        private static PurchaseOrder getPurchaseOrders(SqlDataReader reader)
        {
            return new PurchaseOrder
            {
                ID = (Guid)reader["ID"],
                IDSupplier = (Guid)reader["IDSupplier"],
                Number = (int)reader["Number"],
                Date = DateOnly.FromDateTime((DateTime)reader["Date"]),
                Status = Enum.TryParse<PurchaseStatus>(reader["State"].ToString(), out var state)
                    ? state
                    : PurchaseStatus.Validated,
                Chapter = reader["Chapter"].ToString(),
                Article = reader["Article"].ToString(),
                TypeBudget = reader["TypeBudget"].ToString(),
                TypeService = reader["TypeService"].ToString(),
                THT = (decimal)reader["THT"],
                TVA = (decimal)reader["TVA"],
                TTC = (decimal)reader["TTC"],
                CompletionDelay = (int)reader["CompletionDelay"],
                B= reader["B"].ToString(),
                Fi= reader["FI"].ToString()
            };
        }

        private static PurchaseOrderInfo getPurchaseOrdersInfo(SqlDataReader reader)
        {
            return new PurchaseOrderInfo
            {
                ID = (Guid)reader["ID"],
                IDSupplier = (Guid)reader["IDSupplier"],
                Number = (int)reader["Number"],
                Status = Enum.TryParse<PurchaseStatus>(reader["State"].ToString(), out var state)
                    ? state
                    : PurchaseStatus.Validated,
                SupplierName = reader["SupplierName"].ToString(),
                Date = DateOnly.FromDateTime((DateTime)reader["Date"])
            };
        }

        public async Task<List<PurchaseOrderInfo>> SelectPurchaseOrderInfo()
        {
            var purchaseOrders = new List<PurchaseOrderInfo>();
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(selectPurchaseOrdersInfo, sqlConnection);
                await sqlConnection.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var purchaseOrder = getPurchaseOrdersInfo(reader);
                    purchaseOrders.Add(purchaseOrder);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return purchaseOrders;
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
                cmd.Parameters.AddWithValue("@Status",PurchaseStatus.Editing.ToString());
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


        public async Task<List<PurchaseOrder>> SelectPurchaseOrdersByDate(DateOnly selectedDate)
        {
            var purchaseOrders = new List<PurchaseOrder>();
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(selectAllPurchaseOrderByDate, sqlConnection);

                cmd.Parameters.AddWithValue("@SelectedDate", selectedDate.ToDateTime(TimeOnly.MinValue));

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
        public async Task<PurchaseOrder?> SelectPurchaseOrdersByID(Guid id)
        {
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                await sqlConnection.OpenAsync(); 

                using var cmd = new SqlCommand(selectPurchceOrderByIdQuery, sqlConnection);
                cmd.Parameters.AddWithValue("@ID", id);

                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new PurchaseOrder
                    {
                        ID = (Guid)reader["ID"],
                        IDSupplier = (Guid)reader["IDSupplier"],
                        Number = (int)reader["Number"],
                        Date = DateOnly.FromDateTime((DateTime)reader["Date"]),
                        Status = Enum.TryParse<PurchaseStatus>(reader["State"].ToString(), out var state)
                            ? state
                            : PurchaseStatus.Validated,
                        Chapter = reader["Chapter"].ToString(),
                        Article = reader["Article"].ToString(),
                        TypeBudget = reader["TypeBudget"].ToString(),
                        TypeService = reader["TypeService"].ToString(),
                        THT = (decimal)reader["THT"],
                        TVA = (decimal)reader["TVA"],
                        TTC = (decimal)reader["TTC"],
                        CompletionDelay = (int)reader["CompletionDelay"],
                        B= reader["B"].ToString(),
                        Fi= reader["FI"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching purchase order: {ex.Message}");
                // Log the error (you can use a logger here)
            }

            return null; // Return null if no data is found or an error occurs
        }




        public async Task<(PurchaseOrder, Supplier, List<ProductPdf>)> SelectPurchaseOrderDetails(
            int purchaseOrderNumber)
        {
            var purchaseOrder = new PurchaseOrder();
            var supplier = new Supplier();
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
                    purchaseOrder = getPurchaseOrderData(reader);
                }

                await reader.NextResultAsync();
                while (await reader.ReadAsync())
                {
                    supplier = getSupplierData(reader);
                }

                await reader.NextResultAsync();
                while (await reader.ReadAsync())
                {
                    var product_pdf = getProductPdfData(reader);
                    productPdf.Add(product_pdf);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                throw;
            }

            return (purchaseOrder, supplier, productPdf);
        }

        
       

        private static OrderDetail getAllOrderDetail(SqlDataReader reader)
        {
            return new OrderDetail
            {
                ID = (Guid)reader["ID"],
                IDPurchaseDetail = (Guid)reader["IDPurchaseOrder"],
                IdProducts = (Guid)reader["IdProducts"],
                Quantity = (int)reader["Quantity"],
                UnitPrice = (decimal)reader["UnitPrice"],
                TVA = reader["TVA"].ToString()
            };
        }
     


        public async Task<int> InsertOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(insertOrderDetail, sqlConnection);
                await sqlConnection.OpenAsync();

                cmd.Parameters.AddWithValue("@ID", orderDetail.ID);
                cmd.Parameters.AddWithValue("@IDPurchaseDetail", orderDetail.IDPurchaseDetail);
                cmd.Parameters.AddWithValue("@IdProducts", orderDetail.IdProducts);
                cmd.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", orderDetail.UnitPrice);
                cmd.Parameters.AddWithValue("@TVA", orderDetail.TVA);

                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<OrderDetail>> SelectAllOrderDetail()
        {
            var orderDetails = new List<OrderDetail>();
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(selectAllOrderDetails, sqlConnection);
                await sqlConnection.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    orderDetails.Add(getAllOrderDetail(reader));
                }

                return orderDetails;
            }
            catch (Exception e)
            {
                throw;
            }
        }
           private static ProductPdf getProductPdfData(SqlDataReader reader)
        {
            return new ProductPdf
            {
                Designation = reader["Designation"].ToString(),
                Unitmesure = reader["UnitMeasure"].ToString(),
                Quantity = (int)reader["Quantity"],
                Price = (decimal)reader["UnitPrice"],
                TVA = reader["OrderDetailTVA"].ToString()
            };
        }
        private static Supplier getSupplierData(SqlDataReader reader)
        {
            return new Supplier
            {
                ID = (Guid)reader["ID"],
                RC = reader["RC"].ToString(),
                NIS = (int)reader["NIS"],
                RIB = reader["RIB"].ToString(),
                SupplierName = reader["SupplierName"].ToString(),
                CompanyName = reader["CompanyName"].ToString(),
                AccountName = reader["AccountName"].ToString(),
                Address = reader["Address"].ToString(),
                Phone = reader["Phone"].ToString(),
                Email = reader["Email"].ToString(),
                ART = (long)reader["ART"],
                NIF = (long)reader["NIF"],
                BankAgency = reader["BankAgency"].ToString(),
                State = Enum.TryParse<SupplierState>(reader["Status"].ToString(), out var state)
                    ? state
                    : SupplierState.Deleted
               
            };
        }
        private static PurchaseOrder getPurchaseOrderData(SqlDataReader reader)
        {
            return new PurchaseOrder
            {
                ID = (Guid)reader["ID"],
                IDSupplier = (Guid)reader["IDSupplier"],
                Number = (int)reader["Number"],
                Date = DateOnly.FromDateTime((DateTime)reader["Date"]),
                Status = Enum.TryParse<PurchaseStatus>(reader["State"].ToString(), out var state)
                    ? state
                    : PurchaseStatus.Validated,
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
        public async Task<int> UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                var cmd = new SqlCommand(updatePurchaseOrderByIDQuery, sqlConnection);
                await sqlConnection.OpenAsync();

                cmd.Parameters.AddWithValue("@ID", purchaseOrder.ID);
                cmd.Parameters.AddWithValue("@Fi", purchaseOrder.Fi);
                cmd.Parameters.AddWithValue("@B", purchaseOrder.B);
                cmd.Parameters.AddWithValue("@State", PurchaseStatus.Validated.ToString());
                return await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}