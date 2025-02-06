using Entity.PurchaseOrderEntity;

namespace App.IPurchaseOrderServices
{
    public interface IPurchaseOrderService
    {
        Task<int> AddPurchaseOrder(PurchaseOrder purchaseOrder);
        Task<List<PurchaseOrder>> GetAllPurchaseOrder();
    }
}