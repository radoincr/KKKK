using INV.App.Receipts;
using INVUIs.Receptions.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace INVUIs.Receptions;

public partial class ReceptionList
{
    private bool CommandSelected = false;

    private ReceptionModel commandshow = new();
    [Parameter] public EventCallback<ReceptionModel> OnCommand { get; set; }
    [Parameter] public List<ReceiptInfo> Receptions { get; set; }
    [Parameter] public RenderFragment Pills { get; set; }
    [Inject] public NavigationManager navigationManager { set; get; }

    public EditContext editContext { get; set; }

    public async Task navigatepage(Guid id)
    {
        Navigation.NavigateTo($"receptions/new/{id}");
    }
}