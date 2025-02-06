using Entity.SupplierEntity;

namespace App.ISupplierService;

public interface ISupplierService
{
    Task<int> AddSupplier(Supplier supplier);
    Task<List<Supplier>> GetAllSupplier();
}