namespace INV.App.Receipts;

public class ReceiptProductInfo
{
    public  Guid ReceptionId { get; set; }
    public  Guid ProductId { get; set; }
    public  int Quantity { get; set; }
    public string Designation { get; set; }
    public decimal UnitPrice { set; get; }   
    public Guid DefaultWareHouseId { get; set; }

}