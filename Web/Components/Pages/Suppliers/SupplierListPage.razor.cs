using INV.App.Suppliers;
using Microsoft.AspNetCore.Components;

namespace Web.Components.Pages.Suppliers;

public partial class SupplierListPage
{
 [Inject] public ISupplierService supplierService { get; set; }

 private List<SupplierInfo> suppliers;

 protected override async Task OnInitializedAsync()
 {
    
     suppliers = await supplierService.GetAllSupplier();
 }
}