using INV.App.PurchaseOrders;
using INV.Domain.Entities.ProductPDF;
using INV.Domain.Entities.PurchaseOrders;
using INV.Domain.Entities.SupplierEntity;

namespace INV.Infrastructure.Storage.PurchaseOrderStorages
{
    public interface IPurchaseOrderStorage
    {
        Task<int> InsertPurchaseOrder(PurchaseOrder purchaseOrder);
    
        
        Task<(PurchaseOrder,Supplier,List<ProductPdf>)> SelectPurchaseOrderDetails(int purchaseOrderNumber);
        Task<List<PurchaseOrder>> SelectPurchaseOrdersByDate(DateOnly dateOnly);
        Task<List<PurchaseOrderInfo>> SelectPurchaseOrderInfo();
        
        Task<int> InsertOrderDetail(OrderDetail orderDetail);
        Task<List<OrderDetail>> SelectAllOrderDetail();
        Task<List<PurchaseOrder>> SelectPurchaseOrdersByIDSupplier(Guid IDSupplier);

        Task<PurchaseOrder> SelectPurchaseOrdersByID(Guid id);
        Task <int>UpdatePurchaseOrder(PurchaseOrder purchaseOrder);

    }
}