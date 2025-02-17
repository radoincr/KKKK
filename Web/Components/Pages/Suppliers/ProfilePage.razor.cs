using INV.App.Suppliers;
using INV.Domain.Entities.SupplierEntity;
using Microsoft.AspNetCore.Components;
namespace Web.Components.Pages.Suppliers
{
    public partial class ProfilePage
    {
        [Parameter] public Guid id { get; set; }
        private Supplier Supplier { get; set; } = new();
        [Inject] public ISupplierService serviceSupplier { set; get; }

        protected override async Task OnInitializedAsync()
        {
            Supplier = await serviceSupplier.GetSupplierByID(id);
        }
    }
}