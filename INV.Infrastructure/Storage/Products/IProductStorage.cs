using INV.Domain.Entities.Products;
using INV.Domain.Entities.Purchases;

namespace INV.Infrastructure.Storage.Products;

public interface IProductStorage
{
    Task<int> InsertProduct(Product product);
    ValueTask InsertProductPurchase(PurchaseProduct purchaseProduct);
    Task<int> UpdateProduct(Product product);
    Task<int> DeleteProduct(Guid id);
    Task<List<Product>> SelectProducts();
    Task<bool> ProductExistsByaDesignation(string designation);
}