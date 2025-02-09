namespace INV.App.PurchaseOrders;

public class PurchaseOrderInfo
{
    public Guid ID { set; get; }
    public Guid IDSupplier { set; get; }
    public string SupplierName { set; get; }
    public int Number { set; get; }
    public DateOnly Date { set; get; }
    public string State { set; get; }
}