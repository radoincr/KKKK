using System.Transactions;
using INV.App.Receipts;
using INV.Domain.Entities.Receipts;
using INV.Domain.Shared;
using INV.Infrastructure.Storage.Receipts;

namespace INV.App.Services;

public class ReceiptService : IReceiptService
{
    private readonly IReceiptStorage receiptStorage;

    public ReceiptService(IReceiptStorage receiptStorage)
    {
        this.receiptStorage = receiptStorage;
    }

    public async ValueTask<ReceiptInfo> CreateReceiptFromPurchase(Guid purchaseId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var receipt = await receiptStorage.CreateReceiptFromPurchase(purchaseId);
            return receipt;
        }
    }

    public async ValueTask<Result> ValidateReceipt(Guid receiptId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                await receiptStorage.ValidateReceipt(receiptId);
                scope.Complete();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }
    }

    public async ValueTask<Result<List<ReceiptInfo>>> GetAllReceipts()
    {
        try
        {
            var receipts = await receiptStorage.SelectAllReceipts();
            return receipts;
        }
        catch (Exception ex)
        {
            return Error.Exception(ex);
        }
    }

    public async ValueTask<Result<Receipt?>> GetReceiptById(Guid id)
    {
        try
        {
            var receipt = await receiptStorage.SelectReceiptById(id);

            return receipt is null ? ReceiptError.ReceiptNotFound(id) : receipt;
        }
        catch (Exception ex)
        {
            return Error.Exception(ex);
        }
    }

    public async ValueTask<Result<List<Receipt>>> GetReceiptsByPurchaseId(Guid purchaseId)
    {
        try
        {
            var receipts = await receiptStorage.SelectReceiptsByPurchaseId(purchaseId);
            return receipts;
        }
        catch (Exception ex)
        {
            return Error.Exception(ex);
        }
    }

    public async ValueTask<Result> CreateReceipt(Receipt receipt)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                await receiptStorage.InsertReceipt(receipt);
                foreach (var product in receipt.Products) await receiptStorage.InsertReceiptProduct(product);
                scope.Complete();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Exception(ex);
            }
        }
    }

    public async ValueTask<Result> UpdateReceipt(Receipt receipt)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                await receiptStorage.UpdateReceipt(receipt);
                foreach (var product in receipt.Products) await receiptStorage.UpdateReceiptProduct(product);
                scope.Complete();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ReceiptError.ReceiptUpdateFailed);
            }
        }
    }

    public async ValueTask<Result> RemoveReceipt(Guid id)
    {
        try
        {
            await receiptStorage.DeleteReceipt(id);
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
            var receiptProducts = await receiptStorage.SelectAllReceiptProducts();
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
            var receiptProducts = await receiptStorage.SelectProductsByReceptionId(receptionId);
            return receiptProducts;
        }
        catch (Exception ex)
        {
            return Result.Failure<List<ReceiptProduct>>(ReceiptError.ReceiptProductNotFound(receptionId));
        }
    }

    public async ValueTask<Result> RemoveReceiptProductAsync(Guid receptionId, Guid productId)
    {
        try
        {
            await receiptStorage.DeleteReceiptProduct(receptionId, productId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ReceiptError.ReceiptProductDeletionFailed);
        }
    }

    public async ValueTask<ReceiptInfo> GetReceiptInfoById(Guid receiptId)
    {
        var receiptInfo = await receiptStorage.GetReceiptInfoById(receiptId, true);
        /*if (receiptInfo == null)
            {
                return ReceiptError.ReceiptNotFound(receiptId);
            }*/
        return receiptInfo;
    }

    private async Task<List<Error>> ValidateReceiptCreate(Guid purchaseId)
    {
        var errors = new List<Error>();

        var purchase = await receiptStorage.SelectReceiptsByPurchaseId(purchaseId);
        if (purchase == null || !purchase.Any())
            errors.Add(ReceiptError.ReceiptNotFound(purchaseId));

        var purchaseOrder = await receiptStorage.SelectReceiptById(purchaseId);
        if (purchaseOrder == null || purchaseOrder.Status != ReceiptStatus.validated)
            errors.Add(ReceiptError.InvalidReceiptStatus(ReceiptStatus.editing));

        var existingReceipts = await receiptStorage.SelectReceiptsByPurchaseId(purchaseId);
        if (existingReceipts.Any())
            errors.Add(ReceiptError.ReceiptAlreadyValidated(purchaseId));

        return errors;
    }
}