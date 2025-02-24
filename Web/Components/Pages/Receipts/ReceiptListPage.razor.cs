using INV.App.Purchases;
using INV.App.Services;
using INV.Domain.Entities.Receipts;
using INV.Domain.Shared;
using INVUIs.Purchases;
using INVUIs.Receptions;
using INVUIs.Receptions.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace INV.Web.Components.Pages.Receipts;

public partial class ReceiptListPage
{
    [Inject] public IPurchaseOrderService purchaseService { get; set; }
    [Inject] public IReceiptService ReceiptService { get; set; }
    [Inject] public NavigationManager navigationManager { set; get; }
    private List<PurchaseOrderInfo> purchases;

    protected override async Task OnInitializedAsync()
    {
        purchases = await purchaseService.GetPurchasesForReceiptCreation();
    }

    private PurchaseSelector purchaseSelector;
  
    private async Task CommandSelectednew(Guid purchaseId)
    {

       Guid result=await ReceiptService.CreateReceiptFromPurchase(purchaseId);
       navigationManager.NavigateTo($"/reception/{result}");
    }
}