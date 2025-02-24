using INV.App.Purchases;
using INV.App.Receipts;
using INV.App.Services;
using INV.Domain.Entities.Purchases;
using INV.Domain.Entities.Receipts;
using INV.Domain.Shared;
using INVUIs.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Receptions
{
    public partial class NewReception
    {
        [Inject] public IPurchaseOrderService PurchaseOrderService { get; set; }

        [Inject] public IReceiptService receptionService { get; set; }

        [Parameter]public ReceiptInfo ReceiptInfo { get; set; }
    

        protected override async Task OnInitializedAsync()
        {
        }

        private async Task OnPurchaseOrderChanged(ChangeEventArgs e)
        {
            if (Guid.TryParse(e.Value.ToString(), out Guid parsedGuid))
            {
            }
        }

        private void Create()
        {
            throw new NotImplementedException();
        }
    }
}