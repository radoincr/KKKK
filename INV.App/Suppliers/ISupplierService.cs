using INV.Domain.Entity.SupplierEntity;

namespace INV.App.Suppliers;

public interface ISupplierService
{
    Task<int> AddSupplier(Supplier supplier);
    Task<List<SupplierInfo>> GetAllSupplier();
}