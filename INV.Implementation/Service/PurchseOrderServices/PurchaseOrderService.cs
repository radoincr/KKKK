using INV.App.Products;
using INV.App.PurchaseOrders;
using INV.Domain.Entities.ProductEntity;
using INV.Domain.Entities.ProductPDF;
using INV.Domain.Entities.PurchaseOrders;
using INV.Domain.Entities.SupplierEntity;
using INV.Infrastructure.Storage.ProductsStorages;
using INV.Infrastructure.Storage.PurchaseOrderStorages;

namespace INV.Implementation.Service.PurchseOrderServices;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderStorage purchaseOrderStorage;

    private readonly IProductStorage productStorage;
    public PurchaseOrderService(IPurchaseOrderStorage purchaseOrderStorage,IProductStorage productStorage)
    {
        this.purchaseOrderStorage = purchaseOrderStorage;
        this.productStorage = productStorage;
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
    public async Task <int> ValicatePurchaseOrder(PurchaseOrder purchaseOrder)
    {
        if (purchaseOrder == null)
            throw new ArgumentNullException(nameof(purchaseOrder));
       return await purchaseOrderStorage.UpdatePurchaseOrder(purchaseOrder);
    }

    public async Task CreatePurchaseOrder(PurchaseOrder purchaseOrder, List<Product> products)
    {
        if (purchaseOrder == null)
            throw new ArgumentNullException(nameof(purchaseOrder));
        if (products == null)
            throw new ArgumentNullException(nameof(products));
        await purchaseOrderStorage.InsertPurchaseOrder(purchaseOrder);
        foreach (var product in products)
        {
            await productStorage.InsertProduct(product);
        }
    }


}