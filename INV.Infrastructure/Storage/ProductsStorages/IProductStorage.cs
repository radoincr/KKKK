using Entity.ProductEntity;

namespace Intefrace.ProductStorages
{
    public interface IProductStorage
    {
        Task<int> InsertProduct(Product product);

        Task<List<Product>> SelectAllProduct();


    }
}

