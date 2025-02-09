using INV.App.Suppliers;
using INV.Domain.Entity.PurchaseOrderEntity;

namespace INV.Implementation.Service.Suppliers;
using INV.Infrastructure.Storage.SupplierStorages;

public class SupplierOrderService: ISupplierOrderService
{
    public readonly ISupplierOrderStorage supplierOrderstorage;
    

    public SupplierOrderService(ISupplierOrderStorage _supplierOrderStorage)
    {
        supplierOrderstorage = _supplierOrderStorage;
    }
    
    public async Task<List<PurchaseOrder>> GetPurchaseOrdersByIdSupplier(Guid idSupplier)
    {
        if ( idSupplier== null)
            throw new ArgumentNullException(nameof(idSupplier));
      
        return await supplierOrderstorage.SelectPurchaseOrdersByIDSupplier(idSupplier);
    }

}