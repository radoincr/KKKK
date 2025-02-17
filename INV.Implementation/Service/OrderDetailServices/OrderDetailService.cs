using INV.App.IOrderDetailServices;
using INV.Domain.Entities.PurchaseOrders;
using INV.Infrastructure.Storage.PurchaseOrderStorages;

namespace INV.Implementation.Service.OrderDetailServices;

public class OrderDetailService : IOrderDetailService
{
    private IPurchaseOrderStorage orderDetailStorage;
    
    public OrderDetailService(IPurchaseOrderStorage orderDetailStorage)
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