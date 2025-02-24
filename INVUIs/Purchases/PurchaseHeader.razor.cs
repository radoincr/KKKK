using INVUIs.Purchases.PurchaseModels;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Purchases
{
    public partial class PurchaseHeader : ComponentBase
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
}