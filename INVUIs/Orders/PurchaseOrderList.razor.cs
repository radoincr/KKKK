using INV.App.PurchaseOrders;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Orders;

public partial class PurchaseOrderList
{
    [Parameter] public List<PurchaseOrderInfo> purchaseOrderInfos { get; set; }
    [Inject] public IPurchaseOrderService purchaseOrderService { get; set; }
    private string SearchTerm { get; set; }= "";
    private List<PurchaseOrderInfo> displayedItems =>
        purchaseOrderInfos.Where(i    =>
            i.Number.ToString().Contains(SearchTerm) || 
            i.SupplierName.ToString().ToLower().Contains(SearchTerm.ToLower()))
                .ToList(); 

    private void NavigateToPurchaseOrder()
    {
        Navigation.NavigateTo("/purchaseOrder");
    }
}