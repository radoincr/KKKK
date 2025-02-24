using INV.App.Products;
using INV.Domain.Entities.Products;
using Microsoft.AspNetCore.Components;
using INVUIs.Products.ProductsModel;

namespace INVUIs.Products;

public partial class ProductSelector
{
    [Inject] public IProductService productService { set; get; }
    private List<Product> products;
    private ProductModel selectedProductModel;
    private ProductForm productForm = new();
    private string filterText;
    private List<Product> filteredProducts;

    private bool visibility = false;
    [Parameter] public EventCallback<ProductModel> OnProductSelectedEvent { get; set; }
    [Parameter] public EventCallback OnCreateNewProduct { get; set; }
    private Guid? SelectedProductId { get; set; } = null;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public void ShowModal()
    {
        visibility = true;
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        products = await productService.SelectProducts();
        filterProducts();
    }

    private void filterProducts()
    {
        if (string.IsNullOrEmpty(filterText))
            filteredProducts = products.ToList();

        else
            filteredProducts = products
                .Where(p => (p.Designation?.ToLower().Contains(filterText.ToLower()) ?? false))
                .ToList();

        if (!filteredProducts.Any(p => p.Id == Guid.Empty))
            filteredProducts.Insert(0, new Product { Id = Guid.Empty, Designation = "Create Product" });
    }

    public void HideModal()
    {
        visibility = false;
        List<Product> filteredProducts = new List<Product>();
        StateHasChanged();
    }

    private async Task onProductSelected(object value)
    {
        if (value == null) return;

        if (Guid.TryParse(value.ToString(), out Guid productId))
        {
            if (productId == Guid.Empty)
            {
                SelectedProductId = null;
                await showProductForm();
            }
            else
            {
                SelectedProductId = productId;
                var product = products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    selectedProductModel = new ProductModel
                    {
                        ID = product.Id,
                        Designation = product.Designation,
                        UnitMeasure = product.UnitMeasure,
                        TVA = product.TVA,
                        UnitPrice = product.UnitPrice,
                        Quantity = product.Quantity
                    };
                  //  await OnProductSelectedEvent.InvokeAsync(selectedProductModel);
                }
            }

            StateHasChanged();
        }
    }

    private async Task onSelectClicked()
    {
        if (selectedProductModel != null)
        {
            selectedProductModel.UnitPrice = UnitPrice;
            selectedProductModel.Quantity = Quantity;
          //  await OnProductSelectedEvent.InvokeAsync(selectedProductModel);
            HideModal();
        }
    }
    private async Task Save()
    {
        if (selectedProductModel != null)
        {
            selectedProductModel.UnitPrice = UnitPrice;
            selectedProductModel.Quantity = Quantity;
            await OnProductSelectedEvent.InvokeAsync(selectedProductModel);
            HideModal();
        }
    }

    private async Task showProductForm()
    {
        productForm.ShowModal();
        HideModal();
        StateHasChanged();
    }
}