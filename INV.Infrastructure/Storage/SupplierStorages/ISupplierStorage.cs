using INV.Domain.Entities.SupplierEntity;

namespace INV.Infrastructure.Storage.SupplierStorages
{
    public interface ISupplierStorage
    {
        Task<int> InsertSupplier(Supplier supplier);
        Task<List<Supplier>> SelectAllSupplier();
        Task<Supplier> SelectSupplierByID(Guid id);
        Task<int> UpdateSupplier(Supplier supplier);
        Task<bool> SupplierExistsByRC(string rc);
    }
}

