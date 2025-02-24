using INV.App.Purchases;
using INV.Domain.Entities.Products;
using INV.Domain.Entities.Purchases;
using INV.Infrastructure.Storage.Products;
using INV.Infrastructure.Storage.Purchases;

namespace INV.Implementation.Service.Purchses
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderStorage purchaseOrderStorage;

        private readonly IProductStorage productStorage;

        public PurchaseOrderService(IPurchaseOrderStorage purchaseOrderStorage, IProductStorage productStorage)
        {
            this.purchaseOrderStorage = purchaseOrderStorage;
            this.productStorage = productStorage;
        }

        public async Task<int> AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder == null)
                return 0;
            // return await purchaseOrderStorage.InsertPurchaseOrder(purchaseOrder);
            return 1;
        }

     

        public async Task<List<PurchaseOrder>> GetPurchaseOrdersByDate(DateOnly dateOnly)
        {
            if (dateOnly == null)
                throw new ArgumentNullException(nameof(dateOnly));

            return await purchaseOrderStorage.SelectPurchaseOrdersByDate(dateOnly);
        }

        public async Task<List<PurchaseOrderInfo>> GetPurchaseOrderInfo()
        {
            try
            {
                IAsyncEnumerable<PurchaseOrderInfo> result =  purchaseOrderStorage.SelectPurchaseOrderInfo();
                return await result.ToListAsync();
            }
            catch (Exception e)
            {
                throw new($"Purchase Order service error : {e.Message}");
            }
        }

        public async Task<List<PurchaseOrderInfo>> GetPurchaseOrdersByIdSupplier(Guid idSupplier)
        {
            if (idSupplier == null)
                throw new ArgumentNullException(nameof(idSupplier));

            return await purchaseOrderStorage.SelectPurchaseOrdersByIdSupplier(idSupplier);
        }

        public async Task<PurchaseOrder> GetPurchaseOrdersByID(Guid id)
        {
           
            return await purchaseOrderStorage.SelectPurchaseOrdersByID(id);
        }

        public async Task<int> ValicatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder == null)
                throw new ArgumentNullException(nameof(purchaseOrder));
            return await purchaseOrderStorage.ValidatePurchase(purchaseOrder);
        }

        public async Task CreatePurchaseOrder(PurchaseOrder purchaseOrder, List<Product> products)
        {

            await purchaseOrderStorage.InsertPurchaseOrder(purchaseOrder);

            foreach (var product in products)
            {
                await productStorage.InsertProduct(product);
            }
        }

      
        public async ValueTask<List<PurchaseOrderInfo>> GetPurchasesForReceiptCreation()
        {
            try
            {
                return await purchaseOrderStorage.SelectPurchasesForReceiptCreation();
            }
            catch (Exception e)
            {
                throw new($"Purchase Order service error : {e.Message}");
            }
        }
    }
}