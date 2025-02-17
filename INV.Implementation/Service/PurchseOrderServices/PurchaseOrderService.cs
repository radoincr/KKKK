using INV.App.PurchaseOrders;
using INV.Domain.Entities.ProductPDF;
using INV.Domain.Entities.PurchaseOrders;
using INV.Domain.Entities.SupplierEntity;
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
    
    public async Task<(PurchaseOrder, Supplier, List<ProductPdf>)> GetPurchaseOrderDetails(
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
        try
        {
            return await purchaseOrderStorage.SelectPurchaseOrderInfo();
        }
        catch (Exception e)
        {
            throw new($"Purchase Order service error : {e.Message}");
        }
        
    }
    public async Task<List<PurchaseOrder>> GetPurchaseOrdersByIdSupplier(Guid idSupplier)
    {
        if ( idSupplier== null)
            throw new ArgumentNullException(nameof(idSupplier));
      
        return await purchaseOrderStorage.SelectPurchaseOrdersByIDSupplier(idSupplier);
    }
    
    public async Task<PurchaseOrder> GetPurchaseOrdersByID(Guid id)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id));
        return await purchaseOrderStorage.SelectPurchaseOrdersByID(id);
    }
}