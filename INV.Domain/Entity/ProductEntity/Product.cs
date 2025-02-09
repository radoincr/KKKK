using INV.Domain.Entity.OrderDetailsEntity;

namespace INV.Domain.Entity.ProductEntity
{
    public class Product
    {
        public Guid ID { set; get; }
        
        public string Designation { set; get; }
        
        public string UnitMeasure { set; get; }

        public string DefaultTVARate { set; get; }
        
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}