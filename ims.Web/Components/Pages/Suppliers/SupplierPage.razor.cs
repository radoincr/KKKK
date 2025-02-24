using INV.App.PurchaseOrders;
using INV.App.Suppliers;
using INV.Domain.Entities.SupplierEntity;
using Microsoft.AspNetCore.Components;

namespace ims.Web.Components.Pages.Suppliers
{
    public partial class SupplierPage
    {
        [Parameter] public Guid id { get; set; }
        private Supplier Supplier { get; set; } = new();
        private List<PurchaseOrderInfo> purchases;
        [Inject] public ISupplierService serviceSupplier { set; get; }

        [Inject] public IPurchaseOrderService purchaseService { set; get; }

        protected override async Task OnInitializedAsync()
        {
            Supplier = await serviceSupplier.GetSupplierByID(id);

            if (Supplier != null)
            {
                purchases = await purchaseService.GetPurchaseOrdersByIdSupplier(id);
            }
        }
    }
}