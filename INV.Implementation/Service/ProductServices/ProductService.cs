using INV.App.Products;
using INV.Domain.Entities.ProductEntity;
using INV.Infrastructure.Storage.ProductsStorages;

namespace INV.Implementation.Service.ProductServices;

public class ProductService : IProductService
{
    private IProductStorage productStorage;
    
    public ProductService(IProductStorage productStorage)
    {
        this.productStorage = productStorage;
    }
    public async Task<int> createProduct(Product product)
    {
        return await productStorage.InsertProduct(product);
    }
    public async Task<int> SetProducts(Product product)
    {
        return await productStorage.UpdateProduct(product);
    }

    public async Task<int> RemoveProduct(Guid id)
    {
        return await productStorage.DeleteProduct(id);
    }

    public async Task<List<Product>> SelectProductsByPurchaseOrderId(Guid purchaseID)
    {
        return await productStorage.SelectProductsByPurchaseOrderId(purchaseID);
    }
}