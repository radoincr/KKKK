using Entity.OrderDetailsEntity;
using Entity.ProductEntity;
using Entity.PurchaseOrderEntity;

namespace App.IPurchaseOrderServices
{
    public interface IPurchaseOrderService
    {
        Task<int> AddPurchaseOrder(PurchaseOrder purchaseOrder);
        Task<List<PurchaseOrder>> GetAllPurchaseOrder();
        /*Task <int> AddPurchaseOrderWithDetails(PurchaseOrder purchaseOrder, List<Product> products, List<OrderDetail> orderDetails);
    */
        Task GeneratePurchaseOrderPdf(int purchaseOrderNumber,string outputPdfPath, string htmlTemplatePath);
    }
}