using INVUIs.Models.ProductsModel;
using Microsoft.AspNetCore.Components;

namespace INVUIs.BonCommande;

public partial class ProductsCommande : ComponentBase
{
    [Parameter] public EventCallback<List<ProductModel>> OnProductAddProduct { get; set; }

    private List<ProductModel> products = new();
    private bool showPopup = false;
    private ProductModel newProduct = new ProductModel();
    private void OpenPopup()
    {
        showPopup = true;
    }
    private void DeleteProduct(ProductModel product)
    {
        products.Remove(product);
        for (int i = 0; i < products.Count; i++)
        {
            products[i].Number = i + 1;
        }
        OnProductAddProduct.InvokeAsync(products);
    }
    private async Task SaveProduct()
    {
        if (newProduct.Quantity > 0)
        {
            int nextNumber = products.Count + 1;
            products.Add(new ProductModel()
            {
                ID = Guid.NewGuid(),
                Number = nextNumber,
                Designation = newProduct.Designation,
                UnitMeasure = newProduct.UnitMeasure,
                Quantity = newProduct.Quantity,
                UnitPrice = newProduct.UnitPrice,
                TVA = newProduct.TVA,
                TotalPrice= newProduct.Quantity * newProduct.UnitPrice
            });
            closePopup();
            Clear();
            OnProductAddProduct.InvokeAsync(products);
        }
    }
    private async Task Clear()
    {
        newProduct = new ProductModel();
    }
    private void closePopup()
    {
        showPopup = false;
        StateHasChanged();
        Clear();
    }
}