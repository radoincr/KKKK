using INV.Domain.Entity.ProductEntity;

namespace INV.Infrastructure.Storage.ProductsStorages
{
    public interface IProductStorage
    {
        Task<int> InsertProduct(Product product);

        Task<List<Product>> SelectAllProduct();


    }
}

