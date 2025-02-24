using INVUIs.Products.ProductsModel;

namespace INVUIs.Purchases.PurchaseModels
{
    public class PurchaseModel
    {
        public string selectedArticle { get; set; }
        public int delivery_time { get; set; }
        public string description_article { get; set; }
        public string title_chapter { get; set; }
        public string selectedChapter { get; set; }
        public string selectedCategory { get; set; }
        public string selectedService { get; set; }
        public int DeliveryTime { get; set; }
        public List<ProductModel> ProductModels { get; set; } = new List<ProductModel>();

        public decimal TotalHT => ProductModels.Sum(p => p.Quantity * p.UnitPrice);

        public decimal TotalTVA => ProductModels.Sum(p => p.Quantity * p.UnitPrice * (p.TVA)) / 100;

        public decimal TotalTTC => TotalHT + TotalTVA;
    }
}