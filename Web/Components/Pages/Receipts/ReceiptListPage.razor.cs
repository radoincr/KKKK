using INV.App.Purchases;
using INV.App.Receipts;
using INV.App.Services;
using INV.Domain.Entities.Receipts;
using INV.Domain.Entities.WareHouse;
using INV.Domain.Shared;
using INVUIs.Purchases;
using INVUIs.Receptions;
using INVUIs.Receptions.Models;
using INVUIs.WareHouses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace INV.Web.Components.Pages.Receipts;

public partial class ReceiptListPage
{
    [Inject] public IPurchaseOrderService purchaseService { get; set; }
    [Inject] public IReceiptService ReceiptService { get; set; }
    [Inject] public NavigationManager navigationManager { set; get; }
    private List<PurchaseOrderInfo> purchases;
    private WareHouseForm wareHouseForm;
    private List<ReceiptInfo> receipts = new List<ReceiptInfo>();

    protected override async Task OnInitializedAsync()
    {
        purchases = await purchaseService.GetPurchasesForReceiptCreation();

        var result = await ReceiptService.GetAllReceipts();
        if (result.IsSuccess)
        {
            receipts = result.Value;
        }
    }

    private PurchaseSelector purchaseSelector;

    private async Task CommandSelectednew(Guid purchaseId)
    {
        navigationManager.NavigateTo($"/receptions/new/{purchaseId}");
    }
}