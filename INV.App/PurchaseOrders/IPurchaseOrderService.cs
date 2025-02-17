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

    }
}