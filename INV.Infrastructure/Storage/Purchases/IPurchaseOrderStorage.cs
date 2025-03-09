using INV.App.Purchases;
using INV.Domain.Entities.Purchases;

namespace INV.Infrastructure.Storage.Purchases;

public interface IPurchaseOrderStorage
{
    Task<int> InsertPurchaseOrder(PurchaseOrder purchaseOrder);
    Task<List<PurchaseOrder>> SelectPurchaseOrdersByDate(DateOnly dateOnly);
    IAsyncEnumerable<PurchaseOrderInfo> SelectPurchaseOrderInfo();

    Task<int> InsertPurchaseProduct(PurchaseProduct orderDetail);
    Task<List<PurchaseProduct>> SelectAllPurchaseProduct();
    Task<List<PurchaseOrderInfo>> SelectPurchaseOrdersByIdSupplier(Guid IDSupplier);

    Task<PurchaseOrder> SelectPurchaseOrdersByID(Guid id);
    Task<int> ValidatePurchase(PurchaseOrder purchaseOrder);

    ValueTask<List<PurchaseOrderInfo>> SelectPurchasesForReceiptCreation();
}