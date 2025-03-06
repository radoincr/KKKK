using INV.App.Products;
using INV.Domain.Entities.Products;
using INV.Domain.Shared;
using INVUIs.Products.ProductsModel;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Products
{
    public partial class ProductForm
    {
        private ProductModel newProduct = new ProductModel();
        [Inject] private IProductService productService { set; get; }
        [Inject] private NavigationManager navigationManager { set; get; }
        private List<string> UnitMesures = new() { "U", "KG", "M", "L" };
        private List<int> TVAOptions = new() { 9, 19 };
        private bool visibility = false;
        private Result result;
        private string success = string.Empty;
        private string failure = string.Empty;

        public async Task CreateProduct()
        {
            var product = new Product()
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
                success = "the Product has been added successfully";
            }
            else if (result.IsFailure)
            {
                failure = result.Error.Description;
            }
            StateHasChanged();
            await Task.Delay(1500);

            HideModal();
            StateHasChanged();
        }

        public void ShowModal()
        {
            visibility = true;
            StateHasChanged();
        }

        public void HideModal()
        {
            visibility = false;
            StateHasChanged();
        }
    }
}