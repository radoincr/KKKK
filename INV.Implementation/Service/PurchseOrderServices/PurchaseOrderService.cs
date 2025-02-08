using System.Globalization;
using System.Net;
using System.Text;
using App.IPurchaseOrderServices;
using Entity.ProductPDF;
using Entity.PurchaseOrderEntity;
using Entity.SupplierEntity;
using Interface.PurchaseOrderStorage;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using Service.MyToolServices;

namespace Service.PurchseOrderServices;

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
}