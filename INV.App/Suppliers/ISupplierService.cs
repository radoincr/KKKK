using INV.Domain.Entities.SupplierEntity;

namespace INV.App.Suppliers;

public interface ISupplierService
{
    Task<int> AddSupplier(Supplier supplier);
    Task<List<SupplierInfo>> GetAllSupplier();
    Task<List<SupplierInfo>> GetSupplierByName(string name);
    Task <Supplier> GetSupplierByID(Guid id);
    Task<int> SetSupplier(Supplier supplier);

}