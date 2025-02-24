using INV.App.Receipts;
using INV.Domain.Entities.Purchases;
using INV.Domain.Entities.Receipts;
using INV.Domain.Shared;
using INV.Infrastructure.Storage.Receipts;

namespace INV.App.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptStorage _receiptStorage;

        public ReceiptService(IReceiptStorage receiptStorage)
        {
            _receiptStorage = receiptStorage;
        }

        public async ValueTask<Guid> CreateReceiptFromPurchase(Guid purchaseId)
        {
            try
            {
                /*
                List<Error> errorList = await ValidateReceiptCreate(purchaseId);

                if (errorList.Any())
                    return Result.Failure<Guid>(errorList.First());
                    */

                Guid receiptId = await _receiptStorage.CreateReceiptFromPurchase(purchaseId);
                return(receiptId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async ValueTask<Result> ValidateReceipt(Guid receiptId)
        {
            try
            {
                await _receiptStorage.ValidateReceipt(receiptId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ReceiptError.ReceiptAlreadyValidated(receiptId));
            }
        }

        public async ValueTask<Result<List<Receipt>>> GetAllReceipts()
        {
            try
            {
                var receipts = await _receiptStorage.SelectAllReceipts();
                return Result.Success(receipts);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Receipt>>(ReceiptError.ReceiptNotFound(Guid.Empty));
            }
        }

        public async ValueTask<Result<Receipt?>> GetReceiptById(Guid id)
        {
            try
            {
                var receipt = await _receiptStorage.SelectReceiptById(id);
                return Result.Success(receipt);
            }
            catch (Exception ex)
            {
                return Result.Failure<Receipt?>(ReceiptError.ReceiptNotFound(id));
            }
        }

        public async ValueTask<Result<List<Receipt>>> GetReceiptsByPurchaseId(Guid purchaseId)
        {
            try
            {
                var receipts = await _receiptStorage.SelectReceiptsByPurchaseId(purchaseId);
                return Result.Success(receipts);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<Receipt>>(ReceiptError.ReceiptNotFound(purchaseId));
            }
        }

        public async ValueTask<Result> CreateReceipt(Receipt receipt)
        {
            try
            {
                await _receiptStorage.InsertReceipt(receipt);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ReceiptError.ReceiptCreationFailed);
            }
        }

        public async ValueTask<Result> SetReceipt(Receipt receipt)
        {
            try
            {
                await _receiptStorage.UpdateReceipt(receipt);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ReceiptError.ReceiptUpdateFailed);
            }
        }

        public async ValueTask<Result> RemoveReceipt(Guid id)
        {
            try
            {
                await _receiptStorage.DeleteReceipt(id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ReceiptError.ReceiptDeletionFailed);
            }
        }

        public async ValueTask<Result<List<ReceiptProduct>>> GetAllReceiptProducts()
        {
            try
            {
                var receiptProducts = await _receiptStorage.SelectAllReceiptProducts();
                return Result.Success(receiptProducts);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<ReceiptProduct>>(ReceiptError.ReceiptProductNotFound(Guid.Empty));
            }
        }

        public async ValueTask<Result<List<ReceiptProduct>>> GetProductsByReceptionId(Guid receptionId)
        {
            try
            {
                var receiptProducts = await _receiptStorage.SelectProductsByReceptionId(receptionId);
                return Result.Success(receiptProducts);
            }
            catch (Exception ex)
            {
                return Result.Failure<List<ReceiptProduct>>(ReceiptError.ReceiptProductNotFound(receptionId));
            }
        }

        public async ValueTask<Result> CreateReceiptProduct(ReceiptProduct receiptProduct)
        {
            try
            {
                await _receiptStorage.InsertReceiptProduct(receiptProduct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ReceiptError.ReceiptProductInsertionFailed);
            }
        }

        public async ValueTask<Result> SetReceiptProduct(ReceiptProduct receiptProduct)
        {
            try
            {
                await _receiptStorage.UpdateReceiptProduct(receiptProduct);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ReceiptError.ReceiptProductUpdateFailed);
            }
        }

        public async ValueTask<Result> RemoveReceiptProductAsync(Guid receptionId, Guid productId)
        {
            try
            {
                await _receiptStorage.DeleteReceiptProduct(receptionId, productId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ReceiptError.ReceiptProductDeletionFailed);
            }
        }


        public async ValueTask<ReceiptInfo> GetReceiptInfoById(Guid receiptId)
        {
            try
            {
                var receiptInfo = await _receiptStorage.GetReceiptInfoById(receiptId);
                /*if (receiptInfo == null)
                {
                    return ReceiptError.ReceiptNotFound(receiptId);
                }*/
                return receiptInfo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
     

        public async ValueTask<Result<(List<ReceiptProduct> products, Receipt? receipt, PurchaseOrder? purchaseOrder)>>
            GetReceptionDetails(Guid receptionId)
        {
            try
            {
                var details = await _receiptStorage.GetReceptionDetails(receptionId);
                return Result.Success(details);
            }
            catch (Exception ex)
            {
                return Result.Failure<(List<ReceiptProduct> products, Receipt? receipt, PurchaseOrder? purchaseOrder)>(
                    ReceiptError.ReceiptNotFound(receptionId));
            }
        }

        private async Task<List<Error>> ValidateReceiptCreate(Guid purchaseId)
        {
            List<Error> errors = new List<Error>();

            var purchase = await _receiptStorage.SelectReceiptsByPurchaseId(purchaseId);
            if (purchase == null || !purchase.Any())
                errors.Add(ReceiptError.ReceiptNotFound(purchaseId));

            var purchaseOrder = await _receiptStorage.SelectReceiptById(purchaseId);
            if (purchaseOrder == null || purchaseOrder.Status != ReceiptStatus.validated)
                errors.Add(ReceiptError.InvalidReceiptStatus(ReceiptStatus.editing));

            var existingReceipts = await _receiptStorage.SelectReceiptsByPurchaseId(purchaseId);
            if (existingReceipts.Any())
                errors.Add(ReceiptError.ReceiptAlreadyValidated(purchaseId));

            return errors;
        }
    }
}