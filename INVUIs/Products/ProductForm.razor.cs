using INV.App.Products;
using INV.Domain.Entities.Products;
using INV.Domain.Shared;
using INVUIs.Products.ProductsModel;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Products;

public partial class ProductForm : ComponentBase
{
    private string failure = string.Empty;


    private bool isCreatingProduct = false;

    private ProductModel newProduct = new() { TVA = 19, UnitMeasure = "U" };
    private Result result;
    private string success = string.Empty;
    private List<int> TVAOptions = new() { 9, 19 };
    private List<string> UnitMesures = new() { "U", "KG", "M", "L" };
    private bool visibility = false;
    [Parameter] public EventCallback<Product> OnProductCreated { get; set; }
    [Inject] private IProductService productService { set; get; }
    [Inject] private NavigationManager navigationManager { set; get; }


    public async Task CreateProduct()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            UnitMeasure = newProduct.UnitMeasure,
            Designation = newProduct.Designation,
            TVA = newProduct.TVA,
            UnitPrice = 0,
            Quantity = 0
        };

        result = await productService.CreateProduct(product);

        if (result.IsSuccess)
        {
            success = "The product has been added successfully";
            await OnProductCreated.InvokeAsync(product);
        }
        else
        {
            failure = result.Error.Description;
        }


        HideModal();
        StateHasChanged();
    }

    public void ShowModal()
    {
        visibility = true;
        success = string.Empty;
        failure = string.Empty;
        StateHasChanged();
    }

    public void HideModal()
    {
        isCreatingProduct = false;
        visibility = false;
        success = string.Empty;
        failure = string.Empty;
        StateHasChanged();
    }
}