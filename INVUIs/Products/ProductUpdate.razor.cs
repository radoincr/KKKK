using BlazorBootstrap;
using INV.App.Products;
using INV.Domain.Entities.ProductEntity;
using INVUIs.Models.ProductsModel;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Products;

public partial class ProductUpdate
{
    [Parameter] public ProductModel Product { get; set; }
    [Inject] private IProductService productService { set; get; }
    [Inject] private NavigationManager navigationManager { set; get; }
    
    private Modal modal;
    private ProductModel productModel;
    private Product product;
    
    public async Task ShowProductUpdate()
    {
        await modal?.ShowAsync();
    }

    protected override void OnParametersSet()
    {
        if (Product != null)
        {
            productModel = new ProductModel
            {
                ID = Product.ID,
                IDPurchaseOrder = Product.IDPurchaseOrder,
                Designation = Product.Designation,
                UnitMeasure = Product.UnitMeasure,
                Quantity = Product.Quantity,
                UnitPrice = Product.UnitPrice,
                TVA = Product.TVA,
                DefaultTVARate = Product.DefaultTVARate
            };
        }
        else
        {
            productModel = new ProductModel();
        }
    }

    public async Task UpdateProducts()
    {
        product = new Product()
        {
            ID = productModel.ID,
            IDPurchaseOrder = productModel.IDPurchaseOrder,
            Designation = productModel.Designation,
            UnitMeasure = productModel.UnitMeasure,
            Quantity = productModel.Quantity,
            UnitPrice = productModel.UnitPrice,
            TVA = productModel.TVA,
            DefaultTVARate = productModel.DefaultTVARate,
            TotalePrice = productModel.Quantity * productModel.UnitPrice
        };
        await productService.SetProducts(product);
        navigationManager.NavigateTo(navigationManager.Uri,forceLoad:true);
        CloseModal();
    }

    private async Task CloseModal()
    {
        await modal?.HideAsync();
    }
}
