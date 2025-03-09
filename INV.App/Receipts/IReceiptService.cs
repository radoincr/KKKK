using INV.App.Receipts;
using INV.Domain.Entities.Receipts;
using INV.Domain.Shared;

namespace INV.App.Services;

public interface IReceiptService
{
    ValueTask<ReceiptInfo> CreateReceiptFromPurchase(Guid purchaseId);

    ValueTask<Result> ValidateReceipt(Guid receiptId);

    ValueTask<Result<List<ReceiptInfo>>> GetAllReceipts();

    ValueTask<Result<Receipt?>> GetReceiptById(Guid id);

    ValueTask<Result<List<Receipt>>> GetReceiptsByPurchaseId(Guid purchaseId);

    ValueTask<Result> CreateReceipt(Receipt receipt);

    ValueTask<Result> UpdateReceipt(Receipt receipt);

    ValueTask<Result> RemoveReceipt(Guid id);

    ValueTask<Result<List<ReceiptProduct>>> GetAllReceiptProducts();

    ValueTask<Result<List<ReceiptProduct>>> GetProductsByReceptionId(Guid receptionId);

    ValueTask<Result> RemoveReceiptProductAsync(Guid receptionId, Guid productId);

    ValueTask<ReceiptInfo> GetReceiptInfoById(Guid receiptId);
}