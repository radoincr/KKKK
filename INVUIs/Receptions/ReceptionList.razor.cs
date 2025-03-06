using INV.App.Receipts;
using INV.Domain.Entities.Receipts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using INVUIs.Receptions.Models;
using INV.Domain.Shared;

namespace INVUIs.Receptions
{
    public partial class ReceptionList
    {
        [Parameter] public EventCallback<ReceptionModel> OnCommand { get; set; }
        [Parameter] public List<ReceiptInfo> Receptions { get; set; }
        [Parameter] public RenderFragment Pills { get; set; }
        [Inject] public NavigationManager navigationManager { set; get; }

       

        private ReceptionModel commandshow = new ReceptionModel();
       
        public EditContext editContext { get; set; }
        private bool CommandSelected = false;
        
        

        private async Task CommandSelectednew(Result result)
        {/*
            commandshow = new ReceptionModel
            {
               Date = commandInfo.Date,
               DeliveryNumber = commandInfo.Date.ToString(),
               Status = (ReceptionStatus)commandInfo.Status,

            };
            editContext = new EditContext(commandshow);

           
            StateHasChanged();
            await OnCommand.InvokeAsync(commandshow);*/
        }

        private void NavigateToReceiption(Guid receiptId)
        {
            navigationManager.NavigateTo($"/reception/{receiptId}");

        }
    }
}