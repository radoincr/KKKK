using INV.App.Suppliers;
using INVUIs.Suppliers;
using INVUIs.Suppliers.Models;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Purchases;

public partial class PurchaseSupplier
{
    private bool isSupplierSelected = false;
    public SupplierInfo selectedSupplier = null;
    private SupplierForm supplierForm;
    private List<SupplierInfo> supplierInfo = new();
    private SupplierModel supplierModel = new();
    private bool SupplierSelected = false;
    public SupplierSelector supplierSelector;
    [Parameter] public EventCallback<SupplierInfo> OnSupplierSelected { get; set; }
    [Inject] public ISupplierService supplierService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        supplierInfo = await supplierService.GetAllSupplier();
    }

    private async Task supplierSelected(SupplierInfo supplier)
    {
        if (supplier != null)
        {
            selectedSupplier = supplier;
            await OnSupplierSelected.InvokeAsync(supplier);
            StateHasChanged();
        }
    }
 
}