using INV.Domain.Entity.OrderDetailsEntity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace INV.Infrastructure.Storage.OrderDetailsStorages
{
    public class OrderDetailStorage : IOrderDetailStorage
    {
        private readonly string _connectionString;

        public OrderDetailStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }

        private const string insertOrderDetail = @"
            INSERT INTO OrderDetail (ID, IDPurchaseOrder, IdProducts, Quantity, UnitPrice, TVA)
            VALUES (@ID, @IDPurchaseDetail, @IdProducts, @Quantity, @UnitPrice, @TVA)";

        private const string selectAllOrderDetails = @" SELECT * FROM OrderDetail";

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
    }
}
