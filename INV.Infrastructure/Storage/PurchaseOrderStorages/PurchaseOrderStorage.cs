using Intefrace.PurchaseOrderStorages;
using Microsoft.Extensions.Configuration;

namespace Storage.PurchaseOrderStorages
{
    public class PurchaseOrderStorage : IPurchaseOrderStorage
    {
        private readonly string _connectionString;

        public PurchaseOrderStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }

      
    }
}