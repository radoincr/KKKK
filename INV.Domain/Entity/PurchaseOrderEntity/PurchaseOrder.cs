namespace INV.Domain.Entity.PurchaseOrderEntity
{
    public class PurchaseOrder
    {
        public Guid ID { set; get; }
        
        public Guid IDSupplier { set; get; }

        public int Number { set; get; }
        
        public DateOnly Date { set; get; }
        
        public string Status { set; get; }
        
        public string Chapter { set; get; }
        
        public string Article { set; get; }
        
        public string DesignationArticle { set; get; }
        public string TypeBudget { set; get; }

        public string TypeService { set; get; }

        public Decimal THT { set; get; }

        public Decimal TVA { set; get; }

        public Decimal TTC { set; get; }
        
        public int CompletionDelay { set; get; }

        public string SupplierName { set; get; }
    }
}