using INV.App.PurchaseOrders;
using INV.Domain.Entities.PurchaseOrders;
using INV.Domain.Entities.SupplierEntity;
using INVUIs.Models.PurchaseModels;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Orders.PurchaseOrderDetails
{
    public partial class PurchaseOrderValirdation
    {
        [Parameter] public Supplier? Supplier { get; set; }
        [Parameter] public PurchaseOrder purchaseOrder { get; set; }
        [Inject] private IPurchaseOrderService purchaseOrderService { set; get; }

        [Inject] public NavigationManager navigation { set; get; }


        /*private PurchaseOrder purchaseOrder=new PurchaseOrder();*/
        private PurchaseValidateModel purchaseValidateModel = new PurchaseValidateModel();

        public async Task ValidetePurchase()
        {
            purchaseOrder = new PurchaseOrder()
            {
                ID = purchaseOrder.ID,
                B = purchaseValidateModel.Data,
                Fi = purchaseValidateModel.Data2
            };
            await purchaseOrderService.ValicatePurchaseOrder(purchaseOrder);
            //navigation.NavigateTo(navigation.Uri,forceLoad:true);
            StateHasChanged();
        }
    }
}