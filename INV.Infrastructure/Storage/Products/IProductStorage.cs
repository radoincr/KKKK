using INV.Domain.Entities.Products;

namespace INV.Infrastructure.Storage.Products
{
    public interface IProductStorage
    {

        Task<int> InsertProduct(Product product);
        Task<int> UpdateProduct(Product product);
        Task<int> DeleteProduct(Guid id);
        Task<List<Product>> SelectProducts();
        Task<bool> ProductExistsByaDesignation(string designation);
    }
}

