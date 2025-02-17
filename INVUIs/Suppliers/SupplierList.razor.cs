using INV.App.Suppliers;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Suppliers;

public partial class SupplierList
{
    [Inject] private ISupplierService supplierService { get; set; }
    [Parameter] public List<SupplierInfo> Suppliers { get; set; } = new();
    public List<SupplierInfo> supplierFilter { get; set; } = new();
    private string _searchName = "";
    private SupplierForm supplierSelector = new();
    [Inject] private NavigationManager navigationManager { get; set; }
    private string selectedLink(Guid id) => $"suppliers/{id}";
    private void NavigateToSupplierDetails(Guid supplierId)
    {
        navigationManager.NavigateTo($"/r/{supplierId}");
    }
    protected override void OnParametersSet()
    {
        supplierFilter = Suppliers;
    }

    private string searchName
    {
        get => _searchName;
        set
        {
            _searchName = value;
            getByName();
        }
    }

    private void getByName()
    {
        if (string.IsNullOrWhiteSpace(searchName))
        {
            supplierFilter = Suppliers.ToList();
            return;
        }

        string searchLower = searchName.Trim().ToLower();

        supplierFilter = Suppliers
            .Where(s => s.Name.ToLower().Contains(searchLower) || s.Email.ToLower().Contains(searchLower))
            .OrderBy(s => s.Name)
            .ToList();

        StateHasChanged();
    }

    public void NavigatePage()
    {
        navigationManager.NavigateTo("Supplier");
    }

    public void ShowModal()
    {
        StateHasChanged();
    }
}