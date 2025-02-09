using INV.App.Suppliers;
using INV.Domain.Entity.SupplierEntity;
using INV.Infrastructure.Storage.SupplierStorages;

namespace INV.Implementation.Service.Suppliers;

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

    public async Task<List<SupplierInfo>> GetAllSupplier()
    {
        List<Supplier> result = await supplierstorage.SelectAllSupplier();

        return result.Select(s => new SupplierInfo()
        {
            ID = s.ID,
            Name = s.SupplierName,
            Address = s.Address,
            Phone = s.Phone,
            Email = s.Email

        }).ToList();
        
    }
    public async Task<Supplier> GetSupplierByID(Guid id)
    {
        return await supplierstorage.SelectSupplierByID(id);
    }
}