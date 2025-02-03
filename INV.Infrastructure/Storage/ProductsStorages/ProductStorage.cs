using Intefrace.ProductStorages;
using Microsoft.Extensions.Configuration;

namespace Storage.ProductStorages
{
    public class ProductStorage : IProductStorage
    {
        private readonly string _connectionString;

        public ProductStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }

      
    }
}