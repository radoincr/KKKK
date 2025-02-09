using INV.App.IOrderDetailServices;
using INV.Domain.Entity.OrderDetailsEntity;
using INV.Infrastructure.Storage.OrderDetailsStorages;

namespace INV.Implementation.Service.OrderDetailServices;

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