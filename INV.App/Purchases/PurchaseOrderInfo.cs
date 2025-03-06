using INV.Domain.Entities.Purchases;

namespace INV.App.Purchases
{
    public class PurchaseOrderInfo
    {
        public Guid Id { set; get; }
        public Guid SupplierId { set; get; }
        public string SupplierName { set; get; }
        public string Number { set; get; }
        public DateOnly Date { set; get; }
        public PurchaseStatus Status { get; set; }
    }
}