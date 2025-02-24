using INV.App.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace INVUIs.Suppliers
{
    public partial class SupplierSelector
    {
        [Parameter] public string Title { set; get; }
        [Parameter] public EventCallback<SupplierInfo> OnSelected { set; get; }
        [Parameter] public List<SupplierInfo> Supplier { set; get; }
        [Inject] public ISupplierService supplierService { set; get; }

        private IEnumerable<SupplierInfo> displayedItems = new List<SupplierInfo>();
        private RadzenDataGrid<SupplierInfo> grid;
        private SupplierInfo selectedSupplier = new();

        
        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(1000);
            displayedItems = await supplierService.GetAllSupplier();
            StateHasChanged();
        }

        private async Task selectRowSepplier(SupplierInfo supplier)
        {
            if (supplier != null)
            {
                selectedSupplier = supplier;
                await OnSelected.InvokeAsync(supplier);
            }
        }

    }
}