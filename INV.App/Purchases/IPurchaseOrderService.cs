using INV.Domain.Entities.Products;
using INV.Domain.Entities.Purchases;

namespace INV.App.Purchases
{
    public interface IPurchaseOrderService
    {
        Task<int> AddPurchaseOrder(PurchaseOrder purchaseOrder);

        Task<List<PurchaseOrder>> GetPurchaseOrdersByDate(DateOnly dateOnly);

        Task<List<PurchaseOrderInfo>> GetPurchaseOrderInfo();

        Task<List<PurchaseOrderInfo>> GetPurchaseOrdersByIdSupplier(Guid idSupplier);

        Task<PurchaseOrder> GetPurchaseOrdersByID(Guid id);

        Task<int> ValicatePurchaseOrder(PurchaseOrder purchaseOrder);

        Task CreatePurchaseOrder(PurchaseOrder purchaseOrder, List<Product> products);

        ValueTask<List<PurchaseOrderInfo>> GetPurchasesForReceiptCreation();
    }
}