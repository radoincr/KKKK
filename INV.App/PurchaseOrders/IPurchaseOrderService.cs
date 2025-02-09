using INV.Domain.Entity.PurchaseOrderEntity;

namespace INV.App.PurchaseOrders
{
    public interface IPurchaseOrderService
    {
        Task<int> AddPurchaseOrder(PurchaseOrder purchaseOrder);

        Task<List<PurchaseOrder>> GetPurchaseOrdersByDate(DateOnly dateOnly);
        Task<List<PurchaseOrderInfo>> GetPurchaseOrderInfo();

    }
}