using INV.App.Purchases;
using INV.App.Receipts;
using INV.App.Services;
using INVUIs.Purchases;
using INVUIs.WareHouses;
using Microsoft.AspNetCore.Components;

namespace INV.Web.Components.Pages.Receipts;

public partial class ReceiptListPage
{
    private List<PurchaseOrderInfo> purchases;

    private PurchaseSelector purchaseSelector;
    private List<ReceiptInfo> receipts;
    private WareHouseForm wareHouseForm;
    [Inject] public IPurchaseOrderService purchaseService { get; set; }
    [Inject] public IReceiptService ReceiptService { get; set; }
    [Inject] public NavigationManager navigationManager { set; get; }

    protected override async Task OnInitializedAsync()
    {
        purchases = await purchaseService.GetPurchasesForReceiptCreation();

        var result = await ReceiptService.GetAllReceipts();
        if (result.IsSuccess) receipts = result.Value;
    }

    private async Task CommandSelectednew(Guid purchaseId)
    {
        navigationManager.NavigateTo($"/receptions/new/{purchaseId}");
    }
}