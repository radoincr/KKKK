using INV.App.PurchaseOrders;
using Microsoft.AspNetCore.Components;

namespace ims.Web.Components.Pages.Purchases
{
    public partial class PurchseOrderListPage : ComponentBase
    {
        [Inject] public NavigationManager navigationManager { set; get; }
        [Inject] public IPurchaseOrderService purchaseOrderService { set; get; }
        private List<PurchaseOrderInfo> purchaseOrderInfos;

        protected override async Task OnInitializedAsync()
        {
            purchaseOrderInfos = await purchaseOrderService.GetPurchaseOrderInfo();
            purchaseOrderInfos = purchaseOrderInfos.OrderBy(s => s.Number).ToList();
        }
        private void NavigateToPurchaseOrder() => navigationManager.NavigateTo("/order");
    }
}