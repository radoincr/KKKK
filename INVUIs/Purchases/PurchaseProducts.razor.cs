using INVUIs.Products;
using INVUIs.Products.ProductsModel;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace INVUIs.Purchases;

public partial class PurchaseProducts : ComponentBase
{
    [CascadingParameter] public List<ProductModel> products { set; get; } = new();
    [Parameter] public EventCallback<List<ProductModel>> OnProductAddProduct { get; set; }
    [Parameter] public EventCallback<ProductModel> OnEditProduct { get; set; }
   
    private List<int> TVAOptions = new() { 9, 19 };
    private List<string> UnitMesures = new() { "U", "KG", "M", "L" };
    public RadzenDataGrid<ProductModel> grid;
    
    private ProductForm productForm = new();
    public ProductSelector productSelector = new();
    private ProductModel? selectedProductModel = null;
    public ProductModel productModel { get; set; }
    
    private bool showEditPopup = false;
    private bool showPopup = false;
    private bool Display = false;
    private void OpenPopup() => showPopup = true;
    private void DeleteProduct(ProductModel product)
    {
        products.Remove(product);
        for (var i = 0; i < products.Count; i++) products[i].Number = i + 1;

        OnProductAddProduct.InvokeAsync(products);
    }

    private void Clear() => productModel = new ProductModel();

    private void closePopup()
    {
        showPopup = false;
        StateHasChanged();
        Clear();
    }

    private void AddProductToGrid(ProductModel product)
    {
        product.Number = products.Count + 1;
        product.TotalPrice = product.Quantity * product.UnitPrice;
        products.Add(product);
        StateHasChanged();
    }

    private void CloseEditPopup()
    {
        showEditPopup = false;
        StateHasChanged();
    }

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
            TVA = product.TVA
        };

        showEditPopup = true;
        StateHasChanged();
    }

    private void SaveEditedProduct()
    {
        if (selectedProductModel != null)
        {
            var product = products.FirstOrDefault(p => p.ID == selectedProductModel.ID);
            if (product != null)
            {
                product.Quantity = selectedProductModel.Quantity;
                product.UnitPrice = selectedProductModel.UnitPrice;
                product.TotalPrice = product.Quantity * product.UnitPrice;
            }
            showEditPopup = false;
            StateHasChanged();
        }
    }

    private decimal getTHT()
    {
        return products.Sum(p => p.UnitPrice * p.Quantity);
    }

    private decimal getTVA()
    {
        return products.Sum(p => p.UnitPrice * p.Quantity * p.TVA) / 100;
    }

    private decimal getTTC()
    {
        return getTHT() + getTVA();
    }
}