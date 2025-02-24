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

        public async Task CreateProduct()
        {
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                UnitMeasure = newProduct.UnitMeasure,
                Designation = newProduct.Designation,
                TVA = newProduct.TVA,
                UnitPrice = newProduct.UnitPrice,
                Quantity = newProduct.Quantity
            };
            result = await productService.CreateProduct(product);
            /*if (result.Successed)
            {
                success = "the Product has been added successfully";
            }*/
            await Task.Delay(1500);
            /*navigationManager.NavigateTo(navigationManager.Uri,forceLoad:true);
            */
            StateHasChanged();
            HideModal();
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