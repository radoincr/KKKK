using INV.App.Products;
using INV.App.PurchaseOrders;
using INV.Domain.Entities.ProductEntity;
using INV.Domain.Entities.PurchaseOrders;
using INV.Domain.Entities.SupplierEntity;
using INVUIs.BonCommande;
using INVUIs.Models.ProductsModel;
using INVUIs.Models.PurchaseModels;
using INVUIs.Models.Supplier;
using Microsoft.AspNetCore.Components;
using Web.Components.Layout.Toast;

namespace Web.Components.Pages.PurchaseOrders;

public partial class BonCommande : ComponentBase
{
    public List<ProductModel> productModellist { get; set; }
    public PurchaseModel purchaseModel { get; set; }
    public SupplierModel supplierModel { get; set; }

    private int? activeSection = 0;
    
    private SupplierCommande supBC = new SupplierCommande();
    private DetailsCommande detailsBC = new DetailsCommande();
    private ProductsCommande productsBC = new ProductsCommande();
    private string GetIconClass(int section)
    {
        return activeSection == section ? "bi-chevron-up" : "bi-chevron-down";
    }
    public void GetProduct(List<ProductModel> product) => productModellist = product;

    public void GetPurchase(PurchaseModel purchase) => purchaseModel = purchase;

    public void GetSupplier(SupplierModel supplier) => supplierModel = supplier;
    private void ShowSection(int section)
    {

        activeSection = (activeSection == section) ? null : section;
    }

    private bool isToastVisible = false;
    private ToastType toastType = ToastType.Success;
    private string toastTitle = string.Empty;
    private string toastMessage = string.Empty;

    private void ShowToast(string title, string message, ToastType type)
    {
        toastTitle = title;
        toastMessage = message;
        toastType = type;
        isToastVisible = true;
    }

    private void CloseToast()
    {
        isToastVisible = false;
    }

    [Inject] public IPurchaseOrderService purchaseOrderService { get; set; }
    [Inject] public IProductService productService { get; set; }
    public Product products { get; set; } = new();
    public PurchaseOrder PurchaseOrder { get; set; } = new();
    public Supplier Supplier { get; set; } = new();
    decimal TVA = 0;
    decimal THT = 0;
    decimal TTC = 0;

  
    public async Task create()
    {
        detailsBC.PurchaseModelPass();
        var purchaseOrder = new PurchaseOrder()
        {
            ID = Guid.NewGuid(),
            IDSupplier = supplierModel.ID,
            Behalf = supplierModel.Behalf,
            TypeBudget = purchaseModel.selectedCategory,
            TypeService = purchaseModel.selectedService,
            CompletionDelay = purchaseModel.DeliveryTime,
            Article = purchaseModel.selectedArticle,
            Chapter = purchaseModel.title_chapter,
            Date = DateOnly.FromDateTime(DateTime.Now.Date),
            B = "a",
            Fi = "c",
            TVA = 11,
            TTC = 22,
            THT = 33
        };

        var products = productModellist.Select(pd => new Product()
        {
            ID = Guid.NewGuid(),
            IDPurchaseOrder = purchaseOrder.ID,
            Designation = pd.Designation,
            DefaultTVARate = 19,
            UnitMeasure = pd.UnitMeasure,
            UnitPrice = pd.UnitPrice,
            Quantity = pd.Quantity,
            TVA = pd.TVA,
            TotalePrice = pd.Quantity * pd.UnitPrice
        }).ToList();

        await purchaseOrderService.CreatePurchaseOrder(purchaseOrder, products);
    }
    /*private async Task OnCreate()
   {
       foreach (var pd in products)
       {
           var orderDetail = new OrderDetail()
           {
               UnitPrice = pd.UnitPrice,
               Quantity = pd.Quantity,
               TVA = pd.TVA
           };
           decimal ordertva = decimal.Parse(orderDetail.TVA);
           decimal tvaValue = (orderDetail.UnitPrice * orderDetail.Quantity * ordertva) / 100;
           TVA += tvaValue;
           decimal thtValue = orderDetail.UnitPrice * orderDetail.Quantity;
           THT += thtValue;
       }

       TTC = THT + TVA;


       try
       {
           await purchaseOrderService.AddPurchaseOrder(purchaseOrder);

           foreach (var selectedProduct in products)
           {
               Guid _idproduct = Guid.NewGuid();
               var newProduct = new Product()
               {
                   ID = _idproduct,
                   UnitMeasure = selectedProduct.UnitOfMeasure,
                   DefaultTVARate = 19,
                   Designation = selectedProduct.Data
               };


             //  await productService.AddProduct(newProduct);
               var orderDetail = new OrderDetail()
               {
                   ID = Guid.NewGuid(),
                   IDPurchaseDetail = idPurchaseOrder,
                   UnitPrice = selectedProduct.UnitPrice,
                   Quantity = selectedProduct.Quantity,
                   IdProducts = _idproduct,
                   TVA = selectedProduct.TVA
               };

               await orderDetailService.AddOrderDetail(orderDetail);
           }

           ShowToast("Success", "Purchase Order created successfully", ToastType.Success);
           navigationManager.NavigateTo("/ListPurchaseOrder");
       }
       catch (Exception ex)
       {
           ShowToast("Error", "Error From Purchase Order created ", ToastType.Danger);
           Console.Error.WriteLine($"{ex.Message}");
       }
   }*/
}