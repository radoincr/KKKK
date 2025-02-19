using INV.Domain.Entities.ProductEntity;

namespace INV.App.Products
{
    public interface IProductService
    {
        Task<int> createProduct(Product product);
        Task<int> SetProducts(Product product);
        Task<int> RemoveProduct(Guid id);
        Task<List<Product>> SelectProductsByPurchaseOrderId(Guid purchaseID);
    }
}