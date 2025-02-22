using INV.App.Suppliers;
using INV.Domain.Entities.SupplierEntity;
using INV.Domain.Shared;
using INV.Infrastructure.Storage.SupplierStorages;

namespace INV.Implementation.Service.Suppliers;

public class SupplierService : ISupplierService
{
    public readonly ISupplierStorage supplierstorage;


    public SupplierService(ISupplierStorage _supplierStorage)
    {
        supplierstorage = _supplierStorage;
    }

    public async Task<Result> AddSupplier(Supplier supplier)
    {
        try
        {
            List<ErrorCode> errorList = validateSupplierCreate(supplier);
            if (errorList.Any())
                return Result.Failure(errorList);
            bool RcExsist = await supplierstorage.SupplierExistsByRC(supplier.RC);
            errorList.Clear();
            if (RcExsist)
                errorList.Add(SupplierError.RCExsist);
            if (errorList.Any())
                return Result.Failure(errorList);
            
            await supplierstorage.InsertSupplier(supplier);
            return Result.Succes;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
            Email = s.Email,
            CompanyName = s.CompanyName
        }).ToList();
    }

    public async Task<Supplier> GetSupplierByID(Guid id)
    {
        return await supplierstorage.SelectSupplierByID(id);
    }

    public async Task<List<SupplierInfo>> GetSupplierByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new List<SupplierInfo>();

        List<Supplier> suppliers = await supplierstorage.SelectAllSupplier();

        return suppliers
            .Where(s => s.SupplierName.ToLower().Contains(name.ToLower()) ||
                        s.SupplierName.ToUpper().Contains(name.ToUpper()))
            .Select(s => new SupplierInfo()
            {
                ID = s.ID,
                Name = s.SupplierName,
                Address = s.Address,
                Phone = s.Phone,
                Email = s.Email
            }).ToList();
    }

    public async Task<int> SetSupplier(Supplier supplier)
    {
        return await supplierstorage.UpdateSupplier(supplier);
    }

    private List<ErrorCode> validateSupplierCreate(Supplier supplier)
    {
        List<ErrorCode> errors = new List<ErrorCode>();

        if (string.IsNullOrWhiteSpace(supplier.SupplierName))
            errors.Add(SupplierError.RCExsist);
        return errors;
    }
}