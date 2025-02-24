namespace INV.Web.Services.Suppliers;

public interface IAppSupplierService
{
    ValueTask<SupplierDetail> GetSupplierDetail(Guid id);
}