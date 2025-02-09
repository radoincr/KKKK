using INV.App.PurchaseOrders;
using INV.Domain.Entity.ProductPDF;
using INV.Domain.Entity.PurchaseOrderEntity;
using INV.Domain.Entity.SupplierEntity;
using INV.Infrastructure.Storage.PurchaseOrderStorages;

namespace INV.Implementation.Service.PurchseOrderServices;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderStorage purchaseOrderStorage;

    public PurchaseOrderService(IPurchaseOrderStorage purchaseOrderStorage)
    {
        this.purchaseOrderStorage = purchaseOrderStorage;
    }

    public async Task<int> AddPurchaseOrder(PurchaseOrder purchaseOrder)
    {
        if (purchaseOrder==null)
            return 0; 
        return await purchaseOrderStorage.InsertPurchaseOrder(purchaseOrder);
    }
    
    public async Task<(List<PurchaseOrder>, List<Supplier>, List<ProductPdf>)> GetPurchaseOrderDetails(
        int purchaseOrderNumber)
    {
        return await purchaseOrderStorage.SelectPurchaseOrderDetails(purchaseOrderNumber);
    }

    public async Task<List<PurchaseOrder>> GetPurchaseOrdersByDate(DateOnly dateOnly)
    {
        if (dateOnly == null)
        throw new ArgumentNullException(nameof(dateOnly));
      
        return await purchaseOrderStorage.SelectPurchaseOrdersByDate(dateOnly);
    }

    public async Task<List<PurchaseOrderInfo>> GetPurchaseOrderInfo()
    {
        List<PurchaseOrder> result = await purchaseOrderStorage.SelectPurchaseOrderInfo();
       
        return result.Select(s => new PurchaseOrderInfo()
        {
            ID = s.ID,
            IDSupplier = s.IDSupplier,
            SupplierName = s.SupplierName,
            Number = s.Number,
            Date = s.Date,
            State = s.Status
        }).ToList();    
    }
}