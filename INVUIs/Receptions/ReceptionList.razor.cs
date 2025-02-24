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
        [Parameter] public List<Receipt> receptions { get; set; }
        [Parameter] public RenderFragment Pills { get; set; }

       

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
    }
}