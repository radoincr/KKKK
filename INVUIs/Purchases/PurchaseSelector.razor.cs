using INV.App.Purchases;
using INV.App.Services;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Purchases;

public partial class PurchaseSelector
{
    [Inject] public NavigationManager Navigation { get; set; }
    [Parameter] public List<PurchaseOrderInfo> Command { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public EventCallback<Guid> OnSelected { get; set; }
    private bool displayModal = false;
    private string SearchTerm { get; set; } = "";

    private List<PurchaseOrderInfo> displayedItems =>
        Command?.Where(i =>
                i.Date.ToString("yyyy-MM-dd").ToLower().Contains(SearchTerm?.ToLower() ?? string.Empty) == true ||
                i.SupplierName?.ToLower().Contains(SearchTerm?.ToLower() ?? string.Empty) == true)
            .ToList() ?? new List<PurchaseOrderInfo>();

    private void close()
    {
        displayModal = false;
        StateHasChanged();
    }

    [Inject] public IReceiptService receiptService { set; get; }

    private async void selectItem(PurchaseOrderInfo selected)
    {
        await OnSelected.InvokeAsync(selected.Id);
        close();
    }

    public void ShowModal()
    {
        displayModal = true;
        StateHasChanged();
    }

    public void ShowModal(List<PurchaseOrderInfo> command)
    {
        Command = command;
        ShowModal();
    }
}