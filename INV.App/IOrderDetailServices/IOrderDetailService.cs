using INV.Domain.Entities.PurchaseOrders;

namespace INV.App.IOrderDetailServices
{
    public interface IOrderDetailService
    {
        Task<int> AddOrderDetail(OrderDetail orderDetail);
        Task<List<OrderDetail>> GetAllOrderDetail();
    }
}