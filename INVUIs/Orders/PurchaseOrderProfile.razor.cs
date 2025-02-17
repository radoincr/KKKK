using INV.App.PurchaseOrders;
using INV.Domain.Entities.PurchaseOrders;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Orders;

public partial class PurchaseOrderProfile : ComponentBase
{
    [Parameter] public Guid Id { get; set; }
    private PurchaseOrder purchase;
    [Inject] public IPurchaseOrderService purchaseOrderService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        purchase = await purchaseOrderService.GetPurchaseOrdersByID(Id);
    }
}