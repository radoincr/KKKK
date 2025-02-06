using App.IProductServices;
using Entity.ProductEntity;
using Intefrace.ProductStorages;

namespace Service.ProductServices;

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