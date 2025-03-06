using INV.App.Purchases;
using INV.App.Receipts;
using INV.App.Services;
using INV.Domain.Entities.Purchases;
using INV.Domain.Entities.Receipts;
using INV.Domain.Shared;
using INV.Implementation.Service.Purchses;
using Microsoft.AspNetCore.Components;

namespace INV.Web.Components.Pages.Purchases
{
    public partial class PurchaseDetailPage
    {
        [Parameter] public Guid Id { get; set; }
        [Inject] public IPurchaseOrderService purchaseOrderService { set; get; }
        [Inject] public IReceiptService receiptService { set; get; }

        public PurchaseOrder purchaseOrder = new PurchaseOrder();

      //  public List<PurchaseOrderInfo> purchaseOrderListById;

        public List<Receipt> ReceptionsListByPurchase;

        protected override async Task OnInitializedAsync()
        {
            purchaseOrder = await purchaseOrderService.GetPurchaseOrdersByID(Id);

           // purchaseOrderListById = await purchaseOrderService.GetPurchaseOrdersByIdSupplier(purchaseOrder.SupplierId);

            var result = await receiptService.GetReceiptsByPurchaseId(purchaseOrder.Id);
            if(result.IsSuccess)
            {
                ReceptionsListByPurchase = result.Value;
            }

        }

       
    }
}