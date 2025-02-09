using INV.Domain.Entity.OrderDetailsEntity;

namespace INV.Infrastructure.Storage.OrderDetailsStorages
{
    public interface IOrderDetailStorage
    {
        Task<int> InsertOrderDetail(OrderDetail orderDetail);
        Task<List<OrderDetail>> SelectAllOrderDetail();
    }
}