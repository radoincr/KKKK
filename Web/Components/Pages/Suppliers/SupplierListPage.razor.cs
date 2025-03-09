using INV.App.Suppliers;
using Microsoft.AspNetCore.Components;

namespace INV.Web.Components.Pages.Suppliers;

public partial class SupplierListPage : ComponentBase
{
    private List<SupplierInfo> suppliers;

    private string texte = DateTime.Now.ToString("dd/MM/yyyy HH:ss");
    [Inject] public ISupplierService supplierService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        suppliers = await supplierService.GetAllSupplier();
    }
}