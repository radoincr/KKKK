using Entity.OrderDetailsEntity;
using Entity.ProductPDF;
using Entity.PurchaseOrderEntity;
using Entity.SupplierEntity;

namespace Interface.PurchaseOrderStorage
{
    public interface IPurchaseOrderStorage
    {
        Task<int> InsertPurchaseOrder(PurchaseOrder purchaseOrder);
        Task<List<PurchaseOrder>> SelectAllPurchaseOrder();
        
        Task<(List<PurchaseOrder>,List<Supplier>,List<ProductPdf>)> SelectPurchaseOrderDetails(int purchaseOrderNumber);
    }
}