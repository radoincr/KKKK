using INV.Domain.Entity.ProductEntity;

namespace INV.App.Products
{
    public interface IProductService
    {
        Task<int> AddProduct(Product product);
        Task<List<Product>> GetAllProduct();
    }
}