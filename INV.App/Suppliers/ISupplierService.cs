using INV.Domain.Entities.Suppliers;
using INV.Domain.Shared;

namespace INV.App.Suppliers;

public interface ISupplierService
{
    Task<Result> AddSupplier(Supplier supplier);
    Task<List<SupplierInfo>> GetAllSupplier();
    Task<List<SupplierInfo>> GetSupplierByName(string name);
    Task<ISupplier> GetSupplierByID(Guid id);
    Task<int> SetSupplier(Supplier supplier);
}