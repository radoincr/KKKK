using INV.Domain.Entities.Suppliers;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Suppliers
{
    public partial class SupplierCard
    {
        [Parameter] public ISupplier Supplier { get; set; }
        private bool moreInfoVisible = false;
        private SupplierForm supplierForm;

        private void showMore()
        {
            moreInfoVisible = !moreInfoVisible;
            StateHasChanged();
        }

        
    }
}