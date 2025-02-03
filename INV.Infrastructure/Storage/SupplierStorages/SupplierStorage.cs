using Interface.SupplierStorages;
using Microsoft.Extensions.Configuration;

namespace Storage.SupplierStorages
{
    public class SupplierStorage : ISupplierStorage
    {
        private readonly string _connectionString;

        public SupplierStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }
    }
}