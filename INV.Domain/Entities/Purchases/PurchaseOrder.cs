using INV.Domain.Entities.Budget;

namespace INV.Domain.Entities.Purchases
{
    public class PurchaseOrder
    {
        public Guid Id { set; get; }
        public string Number { set; get; }

        public Guid SupplierId { set; get; }

        //public SupplierInfo Supplier{ get; set; }

        public DateOnly Date { set; get; }

        public string BudgeArticle { set; get; }

        public BudgeType BudgeType { set; get; }
        public ServiceType ServiceType { set; get; }

        public decimal TotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal TotalTTC { get; set; }

        //public decimal TotalHT => Products.Sum(p => p.Quantity * p.UnitPrice);

        //public decimal TotalTVA => Products.Sum(static p => p.Quantity * p.UnitPrice * (p.Product is null ? 0 : p.Product.TVA))/100;

        //public decimal TotalTTC => TotalHT + TotalTVA;

        public int CompletionDelay { set; get; }
        public string? VisaNumber { set; get; } 
        public DateOnly? VisaDate { set; get; }

        public PurchaseStatus Status { set; get; }

        public List<PurchaseProduct> Products { get; set; } = new();
    }
}