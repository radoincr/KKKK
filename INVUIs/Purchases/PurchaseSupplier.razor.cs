using INV.App.Suppliers;
using INVUIs.Suppliers;
using INVUIs.Suppliers.Models;
using Microsoft.AspNetCore.Components;


namespace INVUIs.Purchases
{
    public partial class PurchaseSupplier
    {
        [Parameter] public EventCallback<SupplierModel> OnSupplier { get; set; } 
        [Inject] public ISupplierService supplierService { get; set; }
        private SupplierForm supplierForm;
        public SupplierSelector supplierSelector;
        private SupplierModel supplierModel = new SupplierModel();
        private List<SupplierInfo> supplierInfo = new List<SupplierInfo>();

        private bool SupplierSelected = false;
        private SupplierInfo selectedSupplier = null;

        protected override async Task OnParametersSetAsync()
        {
            supplierInfo = await supplierService.GetAllSupplier();
        }
        private void SupplierSelectednew(SupplierInfo supplier)
        {
            if (supplier != null)
            {
                selectedSupplier = supplier;
                StateHasChanged();
            }
        }

        
    }
}
