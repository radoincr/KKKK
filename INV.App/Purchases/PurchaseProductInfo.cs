namespace INV.App.Purchases;

public class PurchaseProductInfo
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductMU { get; set; }
    public int TVA { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}