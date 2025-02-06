using App.IOrderDetailServices;
using Entity.OrderDetailsEntity;
using Interface.OrderDetailsStorage;

namespace Service.OrderDetailServices;

public class OrderDetailService : IOrderDetailService
{
    private IOrderDetailStorage orderDetailStorage;
    
    public OrderDetailService(IOrderDetailStorage orderDetailStorage)
    {
        this.orderDetailStorage = orderDetailStorage;
    }
    public async Task<int> AddOrderDetail(OrderDetail orderDetail)
    {
         return await orderDetailStorage.InsertOrderDetail(orderDetail);
    }
    public async Task<List<OrderDetail>> GetAllOrderDetail()
    {
        return await orderDetailStorage.SelectAllOrderDetail();
    }
}