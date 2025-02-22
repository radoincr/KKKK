using INV.Domain.Entities.PurchaseOrders;
using INVUIs.Models.PurchaseModels;
using Microsoft.AspNetCore.Components;

namespace INVUIs.BonCommande;

public partial class DetailsCommande : ComponentBase
{
    [Parameter] public EventCallback<PurchaseModel> OnPurchaseOrder { set; get; }
    public PurchaseModel purchaseModel = new PurchaseModel();


    public async Task PurchaseModelPass()
    {
        await OnPurchaseOrder.InvokeAsync(purchaseModel);
    }


    private bool Display = false;

    public void Show()
    {
        Display = !Display;
    }
}