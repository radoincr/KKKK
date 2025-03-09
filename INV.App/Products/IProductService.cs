using INV.Domain.Entities.Products;
using INV.Domain.Shared;

namespace INV.App.Products;

public interface IProductService
{
    Task<Result> CreateProduct(Product product);
    Task<int> SetProducts(Product product);
    Task<int> RemoveProduct(Guid id);
    Task<List<Product>> GetProducts();
}