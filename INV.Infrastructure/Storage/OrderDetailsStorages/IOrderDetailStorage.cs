using Entity.OrderDetailsEntity;

namespace Interface.OrderDetailsStorage
{
    public interface IOrderDetailStorage
    {
        Task<int> InsertOrderDetail(OrderDetail orderDetail);
        Task<List<OrderDetail>> SelectAllOrderDetail();
    }
}