using INV.App.Purchases;
using INV.App.Suppliers;
using INV.Domain.Entities.Budget;
using INV.Domain.Entities.Purchases;
using INVUIs.Products.ProductsModel;
using INVUIs.Purchases;
using INVUIs.Purchases.PurchaseModels;
using Microsoft.AspNetCore.Components;

namespace INV.Web.Components.Pages.Purchases;

public partial class NewPurchasePage : ComponentBase
{
    [Inject] public IPurchaseOrderService purchaseOrderService { get; set; }
    [Inject] public NavigationManager navigationManager { set; get; }
    private readonly List<ProductModel> productModel = new();
    private bool showAlert = false;
    public PurchaseModel purchaseModel { set; get; } = new();
    private SupplierInfo selectedSupplier = new();
    private PurchaseHeader purchaseHeaderRef;
    public string errorMessage { get; set; }

    private async Task create()
    {
        await purchaseHeaderRef.SubmitForm();
        if (productModel is null || productModel.Count == 0)
        {
            showError("Please add at least one product before submitting the order.");
            return;
        }

        if (selectedSupplier.Name is null)
        {
            showError("Please select a supplier.");
            return;
        }

        /*
        if (purchaseModel.DeliveryTime == 0)
        {
            showError(" Required Delivery time.");
            return;
        }
        */

        purchaseModel.ProductModels = productModel;
        
        var purchaseOrder = new PurchaseOrder
        {
            Id = Guid.NewGuid(),
            SupplierId = selectedSupplier.ID,
            BudgeArticle = purchaseModel.description_article,
            BudgeType = (BudgeType)int.Parse(purchaseModel.selectedCategory),
            ServiceType = (ServiceType)int.Parse(purchaseModel.selectedService),
            CompletionDelay = int.Parse(purchaseModel.DeliveryTime),
            Date = DateOnly.FromDateTime(DateTime.Now.Date),
            TotalHT = purchaseModel.TotalHT,
            TotalTVA = purchaseModel.TotalTVA,
            TotalTTC = purchaseModel.TotalTTC
        };

        var products = productModel.Select(pd => new PurchaseProduct
        {
            PurchaseOrderId = purchaseOrder.Id,
            ProductId = pd.ID,
            UnitPrice = pd.UnitPrice,
            Quantity = pd.Quantity
        }).ToList();
        var result = await purchaseOrderService.CreatePurchaseOrder(purchaseOrder, products);
        if (result.IsSuccess)
        {
            navigationManager.NavigateTo("/purchases");
        }
    }
    private async void showError(string message)
    {
        errorMessage = message;
        showAlert = true;
        await Task.Delay(2500);
        showAlert = false;
        errorMessage = "";
        StateHasChanged();
    }
}