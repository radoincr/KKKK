using Entity.PurchaseOrderEntity;

namespace Interface.PurchaseOrderStorage
{
    public interface IPurchaseOrderStorage
    {
        Task<int> InsertPurchaseOrder(PurchaseOrder purchaseOrder);
        Task<List<PurchaseOrder>> SelectAllPurchaseOrder();
    }
}