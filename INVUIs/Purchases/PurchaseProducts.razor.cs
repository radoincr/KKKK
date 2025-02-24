using INVUIs.Products;
using INVUIs.Products.ProductsModel;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace INVUIs.Purchases
{
    public partial class PurchaseProducts : ComponentBase
    {
        private List<string> UnitMesures = new() { "U", "KG", "M", "L" };
        private List<int> TVAOptions = new() { 9, 19 };
        [Parameter] public EventCallback<List<ProductModel>> OnProductAddProduct { get; set; }
        public ProductForm productForm = new ProductForm();
        public ProductSelector productSelector = new ProductSelector();
        private List<ProductModel> products = new();
        public RadzenDataGrid<ProductModel> grid;
        private bool showPopup = false;
        private ProductModel newProduct = new ProductModel();
     

        private void OpenPopup()
        {
            showPopup = true;
        }

        private ProductModel? selectedProductModel = null;
        [Parameter] public EventCallback<ProductModel> OnEditProduct { get; set; }


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

            OnEditProduct.InvokeAsync(selectedProductModel);
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

        public async Task SaveProduct()
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
                    TotalPrice = newProduct.Quantity * newProduct.UnitPrice
                });
                closePopup();
                Clear();
                await OnProductAddProduct.InvokeAsync(products);
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

        private bool Display = false;
        public void Show() => Display = !Display;
        public void Close() => Display = false;

        public async Task ProductPass()
        {
            await OnProductAddProduct.InvokeAsync(products);
        }
        private decimal getTotalPrice()
        {
            return products.Sum(p => p.UnitPrice * p.Quantity);
        }
        private void AddProductToGrid(ProductModel product)
        {
            product.Number = products.Count + 1;
            product.TotalPrice = product.Quantity * product.UnitPrice;
            products.Add(product);
            StateHasChanged();
        }

    }
}