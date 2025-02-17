using INV.App.PurchaseOrders;
using INV.Domain.Entities.PurchaseOrders;
using INV.Domain.Entities.SupplierEntity;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Suppliers.SupplierPofile;

public partial class CommandSettings
{
    [Parameter] public Supplier Supplier { get; set; }
    
    public List<PurchaseOrder> purchase { set; get; } = new();
    [Inject] public IPurchaseOrderService purchaseOrderService { set; get; }

    protected override async Task OnParametersSetAsync()
    {
        purchase = await purchaseOrderService.GetPurchaseOrdersByIdSupplier(Supplier.ID);
    }
}