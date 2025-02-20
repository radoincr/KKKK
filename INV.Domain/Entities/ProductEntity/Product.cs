using INV.Domain.Entities.PurchaseOrders;

namespace INV.Domain.Entities.ProductEntity
{
    public class Product
    {
        public Guid ID { get; set; }
        public Guid IDPurchaseOrder { get; set; }
        public string Designation { get; set; }
        public string UnitMeasure { get; set; }
        public int DefaultTVARate { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TVA { get; set; }

        public decimal TotalePrice { set; get; }
    }
}
/* public Guid ID { set; get; }

         public string Designation { set; get; }

         public string UnitMeasure { set; get; }

         public string DefaultTVARate { set; get; }

         public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
     }*/