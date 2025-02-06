using Entity.SupplierEntity;

namespace Interface.SupplierStorages
{
    public interface ISupplierStorage
    {
        Task<int> InsertSupplier(Supplier supplier);
        
        Task<List<Supplier>> SelectAllSupplier();
    }
}

