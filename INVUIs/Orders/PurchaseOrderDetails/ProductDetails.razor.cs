using INV.App.Products;
using INVUIs.Models.ProductsModel;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Orders.PurchaseOrderDetails;

public partial class ProductDetails
{
    [Parameter] public List<ProductModel> Products { get; set; }
    [Parameter] public EventCallback<ProductModel> OnEditProduct { get; set; }
    [Inject] public IProductService productService { set; get; }
    [Inject] public NavigationManager navigateManager { set; get; }

    private ProductModel? selectedProductModel = null;


    private void EditProduct(ProductModel product)
    {
        selectedProductModel = new ProductModel
        {
            ID = product.ID,
            IDPurchaseOrder = product.IDPurchaseOrder,
            Designation = product.Designation,
            UnitMeasure = product.UnitMeasure,
            Quantity = product.Quantity,
            UnitPrice = product.UnitPrice,
            TVA = product.TVA,
            DefaultTVARate = product.DefaultTVARate
        };

        OnEditProduct.InvokeAsync(selectedProductModel);
    }

    public async Task DeleteProduct(Guid id)
    {
        await productService.RemoveProduct(id);
        navigateManager.NavigateTo(navigateManager.Uri,forceLoad:true);
    }
}