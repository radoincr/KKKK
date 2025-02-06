using Entity.ProductEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.IProductServices
{
    public interface IProductService
    {
        Task<int> AddProduct(Product product);
        Task<List<Product>> GetAllProduct();
    }
}