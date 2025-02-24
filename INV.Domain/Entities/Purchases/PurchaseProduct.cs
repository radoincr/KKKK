
namespace INV.Domain.Entities.Purchases
{
    public class PurchaseProduct
    { 
        public Guid PurchaseOrderId { set; get; }
        public Guid ProductId { set; get; }

        //public Product Product { get; set; }
        public int Quantity { set; get; }
        public decimal UnitPrice { set; get; }   
        
    }
}