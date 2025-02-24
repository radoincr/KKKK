using INV.App.Purchases;
using INV.App.Suppliers;
using INV.Domain.Entities.Suppliers;

namespace INV.Web.Services.Suppliers;

public class AppSupplierService : IAppSupplierService
{
    private readonly ISupplierService supplierService;
    private readonly IPurchaseOrderService purchaseOrderService;

    public AppSupplierService(ISupplierService supplierService, IPurchaseOrderService purchaseOrderService)
    {
        this.supplierService = supplierService;
        this.purchaseOrderService = purchaseOrderService;
    }
    public async ValueTask<SupplierDetail> GetSupplierDetail(Guid id)
    {
        ISupplier supplier = await supplierService.GetSupplierByID(id);

        return new SupplierDetail()
        {
            Id = supplier.Id,
            CompanyName = supplier.CompanyName,
            ManagerName = supplier.ManagerName,
            Address = supplier.Address,
            Phone = supplier.Phone,
            Email = supplier.Email,
            RC = supplier.RC,
            NIS = supplier.NIS,
            ART = supplier.ART,
            RIB = supplier.RIB,
            NIF = supplier.NIF,
            BankAgency = supplier.BankAgency,
            State = supplier.State,
           Purchases = await purchaseOrderService.GetPurchaseOrdersByIdSupplier(supplier.Id)
        };
    }
}