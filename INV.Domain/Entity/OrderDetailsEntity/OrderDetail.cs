namespace Entity.OrderDetailsEntity
{
    public class OrderDetail
    {
        public Guid ID { set; get; }

        public Guid IDPurchaseDetail { set; get; }
        public int IdProducts { set; get; }

        public int Quantity{ set; get; }

        public Decimal UnitPrice { set; get; }

        public int TVA { set; get; }
    }
}