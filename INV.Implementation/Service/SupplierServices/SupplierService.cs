using App.ISupplierService;
using Entity.SupplierEntity;
using Interface.SupplierStorages;
using Storage.SupplierStorages;

namespace Service.SupplierServices;

public class SupplierService : ISupplierService
{
    public readonly ISupplierStorage supplierstorage;
    
    public SupplierService(ISupplierStorage _supplierStorage)
    {
        supplierstorage = _supplierStorage;
    }
    
    public async Task<int> AddSupplier(Supplier supplier)
    {
        return await supplierstorage.InsertSupplier(supplier);
    }
  
    
}