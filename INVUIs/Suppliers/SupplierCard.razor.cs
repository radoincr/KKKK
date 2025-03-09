using INV.Domain.Entities.Suppliers;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Suppliers;

public partial class SupplierCard
{
    private bool moreInfoVisible = false;
    private SupplierForm supplierForm;
    [Parameter] public ISupplier Supplier { get; set; }

    private void showMore()
    {
        moreInfoVisible = !moreInfoVisible;
        StateHasChanged();
    }
}