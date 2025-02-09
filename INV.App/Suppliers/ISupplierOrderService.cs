using INV.Domain.Entity.PurchaseOrderEntity;

namespace INV.App.Suppliers;

public interface ISupplierOrderService
{
    Task<List<PurchaseOrder>> GetPurchaseOrdersByIdSupplier(Guid idSupplier);

}