using INV.App.Receipts;
using INV.Domain.Entities.Receipts;

namespace INV.Infrastructure.Storage.Receipts;

public interface IReceiptStorage
{
    ValueTask<ReceiptInfo> CreateReceiptFromPurchase(Guid purchaseId);

    ValueTask<List<ReceiptInfo>> SelectAllReceipts();

    ValueTask<Receipt?> SelectReceiptById(Guid id);

    ValueTask<List<Receipt>> SelectReceiptsByPurchaseId(Guid purchaseId);

    ValueTask<int> InsertReceipt(Receipt receipt);

    ValueTask<int> UpdateReceipt(Receipt receipt);

    ValueTask<int> DeleteReceipt(Guid id);

    ValueTask<List<ReceiptProduct>> SelectAllReceiptProducts();

    ValueTask<List<ReceiptProduct>> SelectProductsByReceptionId(Guid receptionId);

    ValueTask<int> InsertReceiptProduct(ReceiptProduct receiptProduct);

    ValueTask<int> UpdateReceiptProduct(ReceiptProduct receiptProduct);

    ValueTask<int> DeleteReceiptProduct(Guid receptionId, Guid productId);

    ValueTask<ReceiptInfo> GetReceiptInfoById(Guid receiptId, bool includeProducts = false);

    ValueTask ValidateReceipt(Guid receiptId);
}