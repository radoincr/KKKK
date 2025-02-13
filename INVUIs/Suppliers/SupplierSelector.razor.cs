using Microsoft.AspNetCore.Components;
using INV.App.Suppliers;
namespace INVUIs.Suppliers;

public partial class SupplierSelector 
{
    [Parameter] public List<SupplierInfo> Supplier { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public EventCallback<SupplierInfo> OnSelected { get; set; }
    public SupplierInfo SelectedSupplier = null;
    private bool displayModal = false;
    private string SearchTerm { get; set; } = "";

    private List<SupplierInfo> displayedItems =>
        Supplier.Where(i => i.Name.ToLower().Contains(SearchTerm.ToLower()) ||
                            i.Email.ToString().ToLower().Contains(SearchTerm.ToLower()))
            .ToList();

    private void close()
    {
        Supplier = null;
        displayModal = false;
        StateHasChanged();
    }

    private void selectItem(SupplierInfo selected)
    {
        OnSelected.InvokeAsync(selected);
        close();
    }

    public void ShowModal()
    {
        displayModal = true;
        StateHasChanged();
    }

    public void ShowModal(List<SupplierInfo> Supplier)
    {
        Supplier = Supplier;
        ShowModal();
    }
}