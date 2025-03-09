using INV.App.Purchases;
using INV.App.Receipts;
using INV.App.Services;
using INV.Domain.Entities.Receipts;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Receptions;

public partial class NewReception
{
    private bool statusInput = false;
    [Inject] public IPurchaseOrderService PurchaseOrderService { get; set; }
    [Inject] public IReceiptService receptionService { get; set; }
    [Parameter] public ReceiptInfo ReceiptInfo { get; set; }

    private List<ReceiptProductModel> products { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (ReceiptInfo != null && ReceiptInfo.ReceiptProducts != null)
            products = ReceiptInfo.ReceiptProducts.Select(p => new ReceiptProductModel
            {
                ProductId = p.ProductId,
                UnitPrice = p.UnitPrice,
                Quantity = p.Quantity,
                Designation = p.Designation,
                Received = p.Quantity
            }).ToList();
        else
            // Handle the case where ReceiptInfo or ReceiptProducts is null
            products = new List<ReceiptProductModel>();
    }

    private void Create()
    {
        throw new NotImplementedException();
    }

    private async Task Validate()
    {
        statusInput = true;
        receptionService.ValidateReceipt(ReceiptInfo.Id);
        ReceiptInfo.Status = ReceiptStatus.validated;
        StateHasChanged();
    }

    private void StartEditing()
    {
        statusInput = false;
    }

    private async Task SaveChanges()
    {
        var send = checkInputs();
        if (send)
        {
            var result = await receptionService.GetReceiptById(ReceiptInfo.Id);
            Receipt receiptToSave = new()
            {
                Id = ReceiptInfo.Id,
                Date = (DateOnly)ReceiptInfo.Date,
                DeliveryDate = (DateOnly)ReceiptInfo.DeliveryDate,
                DeliveryNumber = ReceiptInfo.DeliveryNumber,
                PurchaseId = ReceiptInfo.PurchaseId,
                Products = ReceiptInfo.ReceiptProducts.Select(p => new ReceiptProduct
                {
                    ReceptionId = p.ReceptionId,
                    ProductId = p.ProductId,
                    Quantity = products.FirstOrDefault(pp => p.ProductId == pp.ProductId).Received,
                    WareHouseId = p.DefaultWareHouseId
                }).ToList(),
                Status = ReceiptStatus.editing
            };

            if (result != null)
                await receptionService.UpdateReceipt(receiptToSave);
            else
                await receptionService.CreateReceipt(receiptToSave);
            CancelEditing();
        }
    }

    private void CancelEditing()
    {
        statusInput = true;
    }

    private bool checkInputs()
    {
        if (ReceiptInfo.DeliveryDate == null || ReceiptInfo.DeliveryNumber == null) return false;
        return true;
    }
}