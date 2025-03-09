using INV.App.Purchases;
using INV.App.Services;
using INV.Domain.Entities.Purchases;
using INV.Domain.Entities.Receipts;
using Microsoft.AspNetCore.Components;

namespace INV.Web.Components.Pages.Purchases;

public partial class PurchaseDetailPage
{
    public PurchaseOrder purchaseOrder = new();

    //  public List<PurchaseOrderInfo> purchaseOrderListById;

    public List<Receipt> ReceptionsListByPurchase;
    [Parameter] public Guid Id { get; set; }
    [Inject] public IPurchaseOrderService purchaseOrderService { set; get; }
    [Inject] public IReceiptService receiptService { set; get; }

    protected override async Task OnInitializedAsync()
    {
        purchaseOrder = await purchaseOrderService.GetPurchaseOrdersByID(Id);

        // purchaseOrderListById = await purchaseOrderService.GetPurchaseOrdersByIdSupplier(purchaseOrder.SupplierId);

        var result = await receiptService.GetReceiptsByPurchaseId(purchaseOrder.Id);
        if (result.IsSuccess) ReceptionsListByPurchase = result.Value;
    }
}