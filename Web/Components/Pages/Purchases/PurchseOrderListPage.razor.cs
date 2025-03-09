using INV.App.Purchases;
using INV.Shared;
using Microsoft.AspNetCore.Components;

namespace INV.Web.Components.Pages.Purchases;

public partial class PurchseOrderListPage : ComponentBase
{
    private List<PurchaseOrderInfo> purchaseOrderInfos;
    [Inject] public NavigationManager navigationManager { set; get; }
    [Inject] public IPurchaseOrderService purchaseOrderService { set; get; }

    protected override async Task OnInitializedAsync()
    {
        purchaseOrderInfos = await purchaseOrderService.GetPurchaseOrderInfo();
        purchaseOrderInfos = purchaseOrderInfos.OrderBy(s => s.Number).ToList();
    }

    private void NavigateToPurchaseOrder()
    {
        navigationManager.NavigateTo($"{PageRoutes.CreatePurchase}");
    }
}