using INV.Domain.Entities.Receipts;

namespace INV.App.Receipts;
public class ReceiptInfo
{
    public  Guid Id { get; set; }
    public Guid PurchaseId { get; set; }
    public DateOnly? Date { get; set; }    
    public string BudgetArticle { get; set; }
    public string purchaseNumber { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public Guid supplierId { get; set; }
    public  string supplierName { get; set; }
    public string DeliveryNumber { get; set; }
    public  DateOnly? DeliveryDate { get; set; }
    public  ReceiptStatus Status { get; set; }
    public List<ReceiptProductInfo> ReceiptProducts { get; set; }

}