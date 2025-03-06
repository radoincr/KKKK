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
        [Parameter] public ReceiptInfo ReceiptInfo { get; set; }

        private List<ReceiptProductModel> products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            products = ReceiptInfo.ReceiptProducts.Select(p => new ReceiptProductModel()
            {
                ProductId = p.ProductId,
                UnitPrice = p.UnitPrice,
                Quantity = p.Quantity,
                Designation = p.Designation,
                Received = p.Quantity
            }).ToList();
        }

        private void Create()
        {
            throw new NotImplementedException();
        }

        private async Task Validate()
        {
            receptionService.ValidateReceipt(ReceiptInfo.Id);
            ReceiptInfo.Status = ReceiptStatus.validated;
            StateHasChanged();
        }

        private bool isEditing = true;

        private void StartEditing()
        {
            isEditing = true;
        }

        private async Task SaveChanges()
        {
            var result = await receptionService.GetReceiptById(ReceiptInfo.Id);
            Receipt receiptToSave = new()
            {
                Id = ReceiptInfo.Id,
                Date = (DateOnly)ReceiptInfo.Date,
                DeliveryDate = (DateOnly)ReceiptInfo.DeliveryDate,
                DeliveryNumber = ReceiptInfo.DeliveryNumber,
                PurchaseId = ReceiptInfo.PurchaseId,
                Products = ReceiptInfo.ReceiptProducts.Select(p => new ReceiptProduct()
                {
                    ReceptionId = p.ReceptionId,
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList(),
                Status = ReceiptStatus.editing
            };

            if (result.IsSuccess)
            {
               // await receptionService.UpdateReceipt(receiptToSave);

            }
            else
            {
                await receptionService.CreateReceipt(receiptToSave);
                
            }
        }

        private void CancelEditing()
        {
            isEditing = false;
        }
    }
}