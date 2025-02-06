using App.IPurchaseOrderServices;
using Entity.PurchaseOrderEntity;
using Interface.PurchaseOrderStorage;

namespace Service.PurchseOrderServices;

public class PurchaseOrderService: IPurchaseOrderService
{
    private IPurchaseOrderStorage purchaseOrderStorage;
    
    public PurchaseOrderService(IPurchaseOrderStorage purchaseOrderStorage)
    {
        this.purchaseOrderStorage = purchaseOrderStorage;
    }
    public async Task<int> AddPurchaseOrder(PurchaseOrder purchaseOrder)
    {
        return await purchaseOrderStorage.InsertPurchaseOrder(purchaseOrder);
    }
    public async Task<List<PurchaseOrder>> GetAllPurchaseOrder()
    {
        return await purchaseOrderStorage.SelectAllPurchaseOrder();
    }
}