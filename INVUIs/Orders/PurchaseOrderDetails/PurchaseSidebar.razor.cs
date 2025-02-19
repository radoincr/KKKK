using BlazorBootstrap;
using INV.App.PurchaseOrders;
using INV.Domain.Entities.PurchaseOrders;
using INV.Domain.Entities.SupplierEntity;
using INVUIs.Models.PurchaseModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace INVUIs.Orders.PurchaseOrderDetails;

public partial class PurchaseSidebar
{
    [Parameter] public Supplier? Supplier { get; set; }
    [Parameter] public PurchaseOrder purchaseOrder { get; set; }
    [Inject] private IPurchaseOrderService purchaseOrderService { set; get; }

    [Inject] public NavigationManager navigation { set; get; }


    /*private PurchaseOrder purchaseOrder=new PurchaseOrder();*/
    private PurchaseValidateModel purchaseValidateModel = new PurchaseValidateModel();
    List<ToastMessage> messages = new List<ToastMessage>();
    private void ShowMessage(ToastType toastType) => messages.Add(CreateToastMessage(toastType));
    private ToastMessage CreateToastMessage(ToastType toastType)
        => new ToastMessage
        {
            Type = toastType,
            Title = "Validate Purchase Order",
            HelpText = $"{DateTime.Now}",
            Message = $"You will not be able to change " +
                      $" the information again",
            IconName = IconName.Info
        };



    public async Task ValidetePurchase()
    {
        purchaseOrder = new PurchaseOrder()
        {
            ID = purchaseOrder.ID,
            B = purchaseValidateModel.Data,
            Fi = purchaseValidateModel.Data2
        };
        await purchaseOrderService.ValicatePurchaseOrder(purchaseOrder);
        ShowMessage(ToastType.Primary);
        //navigation.NavigateTo(navigation.Uri,forceLoad:true);
        StateHasChanged();
    }
}