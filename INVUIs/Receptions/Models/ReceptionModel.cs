using INV.Domain.Entities.Receipts;


namespace INVUIs.Receptions.Models
{
    public class ReceptionModel
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public DateOnly Date { get; set; }

        public DateOnly DeliveryDate { get; set; }

        public string DeliveryNumber { get; set; }

        public ReceiptStatus Status { get; set; }
    }
}
