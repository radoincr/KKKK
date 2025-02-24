using INV.App.Products;
using INV.App.PurchaseOrders;
using INV.Domain.Entities.Products;
using INV.Domain.Entities.Purchases;
using INV.Domain.Entities.SupplierEntity;
using INVUIs.Products.ProductsModel;
using INVUIs.Purchases.PurchaseModels;
using INVUIs.Suppliers.Models;
using Microsoft.AspNetCore.Components;

namespace ims.Web.Components.Pages.Purchases
{
    public partial class NewPurchasePage : ComponentBase
    {
        public List<ProductModel> productModellist { get; set; }
        public PurchaseModel purchaseModel { get; set; }
        public SupplierModel supplierModel { get; set; }
        private int? activeSection = 0;
        public void GetProduct(List<ProductModel> product) => productModellist = product;
        public void GetPurchase(PurchaseModel purchase) => purchaseModel = purchase;
        public void GetSupplier(SupplierModel supplier) => supplierModel = supplier;
        [Inject] public IPurchaseOrderService purchaseOrderService { get; set; }
        [Inject] public IProductService productService { get; set; }
        public Product products { get; set; } = new();
        public PurchaseOrder PurchaseOrder { get; set; } = new();
        public Supplier Supplier { get; set; } = new();


        public async Task create()
        {
            var purchaseOrder = new PurchaseOrder()
            {
                Id = Guid.NewGuid(),
                SupplierId = supplierModel.ID,
              //  BudgeType = purchaseModel.typ,
              //  ServiceType = purchaseModel.selectedService,
                CompletionDelay = purchaseModel.DeliveryTime,
              //  Article = purchaseModel.selectedArticle,
              //  Chapter = "aaa",
                Date = DateOnly.FromDateTime(DateTime.Now.Date),
            };

            var products = productModellist.Select(pd => new Product()
            {
              /*  ID = Guid.NewGuid(),
                IDPurchaseOrder = purchaseOrder.ID,
                Designation = pd.Designation,
                DefaultTVARate = 19,
                UnitMeasure = pd.UnitMeasure,
                UnitPrice = pd.UnitPrice,
                Quantity = pd.Quantity,
                TVA = pd.TVA,
                TotalePrice = pd.Quantity * pd.UnitPrice*/
            }).ToList();

            await purchaseOrderService.CreatePurchaseOrder(purchaseOrder, products);
        }
    }
}