using INV.App.Suppliers;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Suppliers;

public partial class SupplierList
{
    private string _searchName = "";
    private SupplierForm supplierForm;
    [Inject] private ISupplierService supplierService { get; set; }
    [Parameter] public List<SupplierInfo> Suppliers { get; set; } = new();
    public List<SupplierInfo> supplierFilter { get; set; } = new();
    [Inject] private NavigationManager navigationManager { get; set; }

    private string searchName
    {
        get => _searchName;
        set
        {
            _searchName = value;
            getByName();
        }
    }

    private string selectedLink(Guid id)
    {
        return $"suppliers/{id}";
    }

    private void NavigateToSupplierDetails(Guid supplierId)
    {
        navigationManager.NavigateTo($"/suppliers/{supplierId}");
    }

    protected override void OnParametersSet()
    {
        supplierFilter = Suppliers;
    }

    private void showSupplierForm()
    {
        if (supplierForm != null) supplierForm.ShowModal();
    }

    private void getByName()
    {
        if (string.IsNullOrWhiteSpace(searchName))
        {
            supplierFilter = Suppliers.ToList();
            return;
        }

        var searchLower = searchName.Trim().ToLower();

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