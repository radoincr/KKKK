using INV.Domain.Entity.PurchaseOrderEntity;
using INV.Domain.Entity.SupplierEntity;

namespace INV.Infrastructure.Storage.SupplierStorages;

public interface ISupplierOrderStorage
{
    
        Task<List<PurchaseOrder>> SelectPurchaseOrdersByIDSupplier(Guid IDSupplier);

}