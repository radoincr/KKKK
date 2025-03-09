using INV.Domain.Entities.Purchases;
using INV.Domain.Shared;

namespace INV.App.Purchases;

public interface IPurchaseOrderService
{
    Task<int> AddPurchaseOrder(PurchaseOrder purchaseOrder);

    Task<List<PurchaseOrder>> GetPurchaseOrdersByDate(DateOnly dateOnly);

    Task<List<PurchaseOrderInfo>> GetPurchaseOrderInfo();

    Task<List<PurchaseOrderInfo>> GetPurchaseOrdersByIdSupplier(Guid idSupplier);

    Task<PurchaseOrder> GetPurchaseOrdersByID(Guid id);

    Task<int> ValicatePurchaseOrder(PurchaseOrder purchaseOrder);

    ValueTask<Result> CreatePurchaseOrder(PurchaseOrder purchaseOrder, List<PurchaseProduct> products);

    ValueTask<List<PurchaseOrderInfo>> GetPurchasesForReceiptCreation();
}