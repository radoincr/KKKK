using Entity.OrderDetailsEntity;


namespace App.IOrderDetailServices
{
    public interface IOrderDetailService
    {
        Task<int> AddOrderDetail(OrderDetail orderDetail);
        Task<List<OrderDetail>> GetAllOrderDetail();
    }
}