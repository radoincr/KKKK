using INV.App.PurchaseOrders;
using Microsoft.AspNetCore.Components;

namespace Web.Components.Pages.PurchaseOrders;

public partial class PurchseOrderListPage : ComponentBase
{
    [Inject] public IPurchaseOrderService purchaseOrderService { set; get; }
    private List<PurchaseOrderInfo> purchaseOrderInfos;
    protected override async Task OnInitializedAsync()
    {
    
        purchaseOrderInfos = await purchaseOrderService.GetPurchaseOrderInfo();
    }
}



