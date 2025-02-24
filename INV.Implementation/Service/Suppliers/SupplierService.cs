using INV.App.Suppliers;
using INV.Domain.Entities.Suppliers;
using INV.Domain.Shared;
using INV.Infrastructure.Storage.SupplierStorages;

namespace INV.Implementation.Service.Suppliers
{
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
                List<Error> errorList = await validateSupplierCreate(supplier);
                
                
                if (errorList.Any())
                    return Result.Failure(errorList);

                await supplierstorage.InsertSupplier(supplier);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(Error.Exception(e));
            }
        }

        public async Task<List<SupplierInfo>> GetAllSupplier()
        {
            List<Supplier> result = await supplierstorage.SelectAllSupplier();

            return result.Select(s => new SupplierInfo()
            {
                ID = s.Id,
                Name = s.ManagerName,
                Address = s.Address,
                Phone = s.Phone,
                Email = s.Email,
                CompanyName = s.CompanyName
            }).ToList();
        }

        public async Task<ISupplier> GetSupplierByID(Guid id)
        {
            return await supplierstorage.SelectSupplierByID(id);
        }

        public async Task<List<SupplierInfo>> GetSupplierByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<SupplierInfo>();

            List<Supplier> suppliers = await supplierstorage.SelectAllSupplier();

            return suppliers
                .Where(s => s.ManagerName.ToLower().Contains(name.ToLower()) ||
                            s.CompanyName.ToUpper().Contains(name.ToUpper()))
                .Select(s => new SupplierInfo()
                {
                    ID = s.Id,
                    Name = s.CompanyName,
                    Address = s.Address,
                    Phone = s.Phone,
                    Email = s.Email
                }).ToList();
        }

        public async Task<int> SetSupplier(Supplier supplier)
        {
            return await supplierstorage.UpdateSupplier(supplier);
        }

        private async Task<List<Error>> validateSupplierCreate(Supplier supplier)
        {
            List<Error> errors = new List<Error>();

            

            bool rcExists = await supplierstorage.SupplierExistsByRC(supplier.RC);
            if (rcExists)
                errors.Add(SupplierError.RCExsist(supplier.RC));
                
            bool nisExists = await supplierstorage.SupplierExistsByNIS(supplier.NIS);
            if (nisExists)
                errors.Add(SupplierError.NISExsist);
            bool ribExists = await supplierstorage.SupplierExistsByRIB(supplier.RIB);
            if (ribExists)
                errors.Add(SupplierError.RIBExsist);

            return errors;
        }
    }
}