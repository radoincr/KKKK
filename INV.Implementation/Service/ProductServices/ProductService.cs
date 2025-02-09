using INV.App.Products;
using INV.Domain.Entity.ProductEntity;
using INV.Infrastructure.Storage.ProductsStorages;

namespace INV.Implementation.Service.ProductServices;

public class ProductService : IProductService
{
    private IProductStorage productStorage;
    
    public ProductService(IProductStorage productStorage)
    {
        this.productStorage = productStorage;
    }
    public async Task<int> AddProduct(Product product)
    {
        return await productStorage.InsertProduct(product);
    }
    public async Task<List<Product>> GetAllProduct()
    {
        return await productStorage.SelectAllProduct();
    }
}