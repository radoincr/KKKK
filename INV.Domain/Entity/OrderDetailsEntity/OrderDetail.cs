namespace Entity.OrderDetailsEntity
{
    public class OrderDetail
    {
        public Guid ID { set; get; }
        public Guid IDPurchaseDetail { set; get; }
        public Guid IdProducts { set; get; }
        public int Quantity{ set; get; }
        public Decimal UnitPrice { set; get; }
        public string TVA { set; get; }
    }
}