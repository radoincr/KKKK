using INV.Domain.Entity.SupplierEntity;

namespace INV.Infrastructure.Storage.SupplierStorages
{
    public interface ISupplierStorage
    {
        Task<int> InsertSupplier(Supplier supplier);
        
        Task<List<Supplier>> SelectAllSupplier();
        Task<Supplier> SelectSupplierByID(Guid id);
    }
}

