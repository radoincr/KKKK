using INV.Domain.Entity.ProductPDF;
using INV.Domain.Entity.PurchaseOrderEntity;
using INV.Domain.Entity.SupplierEntity;

namespace INV.Infrastructure.Storage.PurchaseOrderStorages
{
    public interface IPurchaseOrderStorage
    {
        Task<int> InsertPurchaseOrder(PurchaseOrder purchaseOrder);
    
        
        Task<(List<PurchaseOrder>,List<Supplier>,List<ProductPdf>)> SelectPurchaseOrderDetails(int purchaseOrderNumber);
        Task<List<PurchaseOrder>> SelectPurchaseOrdersByDate(DateOnly dateOnly);
        Task<List<PurchaseOrder>> SelectPurchaseOrderInfo();
    }
}