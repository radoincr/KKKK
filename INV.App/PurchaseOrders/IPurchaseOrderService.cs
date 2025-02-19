using INV.Domain.Entities.ProductEntity;
using INV.Domain.Entities.PurchaseOrders;

namespace INV.App.PurchaseOrders
{
    public interface IPurchaseOrderService
    {
        Task<int> AddPurchaseOrder(PurchaseOrder purchaseOrder);

        Task<List<PurchaseOrder>> GetPurchaseOrdersByDate(DateOnly dateOnly);
        Task<List<PurchaseOrderInfo>> GetPurchaseOrderInfo();
        Task<List<PurchaseOrder>> GetPurchaseOrdersByIdSupplier(Guid idSupplier);

        Task<PurchaseOrder> GetPurchaseOrdersByID(Guid id);
        Task<int> ValicatePurchaseOrder(PurchaseOrder purchaseOrder);
        Task CreatePurchaseOrder(PurchaseOrder purchaseOrder, List<Product> products);
    }
}