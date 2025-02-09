using INV.Domain.Entity.OrderDetailsEntity;

namespace INV.App.IOrderDetailServices
{
    public interface IOrderDetailService
    {
        Task<int> AddOrderDetail(OrderDetail orderDetail);
        Task<List<OrderDetail>> GetAllOrderDetail();
    }
}