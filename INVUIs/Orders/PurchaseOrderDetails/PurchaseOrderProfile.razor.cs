using BlazorBootstrap;
using INV.App.Products;
using INV.Domain.Entities.ProductEntity;
using INV.Domain.Entities.PurchaseOrders;
using INV.Domain.Entities.SupplierEntity;
using INVUIs.Models.ProductsModel;
using INVUIs.Products;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Orders.PurchaseOrderDetails;

public partial class PurchaseOrderProfile
{
    [Parameter] public Guid Id { get; set; }
    [Inject] public IProductService productService { get; set; }
    [Inject] protected PreloadService preloadService { get; set; }
    private PurchaseOrder? PurchaseOrder;
    private Supplier? Supplier;
    private List<Product> products = new();
    private List<ProductModel> productModels = new();
    private ProductModel selectedProductModel;
    private ProductUpdate productUpdateComponent;

    private void EditProduct(ProductModel product)
    {
        selectedProductModel = product;
        ShowModal();
    }

    private async Task ShowModal()
    {
        await productUpdateComponent.ShowProductUpdate();
    }

    protected override async Task OnInitializedAsync()
    {
        PurchaseOrder = await PurchaseOrderService.GetPurchaseOrdersByID(Id);
        if (PurchaseOrder != null)
        {
            Supplier = await SupplierService.GetSupplierByID(PurchaseOrder.IDSupplier);
            var products = await productService.SelectProductsByPurchaseOrderId(PurchaseOrder.ID);
            productModels = products.Select(p => ProductPass(p)).ToList();
        }

     
    }

    private ProductModel ProductPass(Product product)
    {
        return new ProductModel
        {
            ID = product.ID,
            IDPurchaseOrder = product.IDPurchaseOrder,
            Designation = product.Designation,
            UnitMeasure = product.UnitMeasure,
            Quantity = product.Quantity,
            UnitPrice = product.UnitPrice,
            TVA = product.TVA,
            DefaultTVARate = product.DefaultTVARate,
            TotalPrice = product.TotalePrice
        };
    }
}