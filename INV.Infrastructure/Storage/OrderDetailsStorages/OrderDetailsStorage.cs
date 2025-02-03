using Interface.OrderDetailsStorages;
using Microsoft.Extensions.Configuration;


namespace Storages.OrderDetailsStorages
{
    public class OrderDetailsStorage : IOrderDetailsStorage
    {
        private readonly string _connectionString;

        public OrderDetailsStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("INV");
        }

      
    }
}