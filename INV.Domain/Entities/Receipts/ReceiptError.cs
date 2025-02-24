using INV.Domain.Shared;

namespace INV.Domain.Entities.Receipts
{
    public static class ReceiptError
    {
        public static Error ReceiptNotFound(Guid receiptId) =>
            Error.NotFound("ReceiptError.ReceiptNotFound", $"Receipt with ID {receiptId} was not found.");

        public static Error InvalidReceiptStatus(ReceiptStatus status) =>
            Error.Conflict("ReceiptError.InvalidReceiptStatus", $"The receipt status {status} is invalid for this operation.");

        public static Error ReceiptAlreadyValidated(Guid receiptId) =>
            Error.Conflict("ReceiptError.ReceiptAlreadyValidated", $"Receipt with ID {receiptId} has already been validated.");

        public static Error ReceiptCreationFailed =>
            Error.Failure("ReceiptError.ReceiptCreationFailed", "Failed to create the receipt.");

        public static Error ReceiptUpdateFailed =>
            Error.Failure("ReceiptError.ReceiptUpdateFailed", "Failed to update the receipt.");

        public static Error ReceiptDeletionFailed =>
            Error.Failure("ReceiptError.ReceiptDeletionFailed", "Failed to delete the receipt.");

        public static Error ReceiptProductNotFound(Guid productId) =>
            Error.NotFound("ReceiptError.ReceiptProductNotFound", $"Receipt product with ID {productId} was not found.");

        public static Error InvalidReceiptProductQuantity(int quantity) =>
            Error.Problem("ReceiptError.InvalidReceiptProductQuantity", $"The quantity {quantity} is invalid for the receipt product.");

        public static Error ReceiptProductInsertionFailed =>
            Error.Failure("ReceiptError.ReceiptProductInsertionFailed", "Failed to insert the receipt product.");

        public static Error ReceiptProductUpdateFailed =>
            Error.Failure("ReceiptError.ReceiptProductUpdateFailed", "Failed to update the receipt product.");

        public static Error ReceiptProductDeletionFailed =>
            Error.Failure("ReceiptError.ReceiptProductDeletionFailed", "Failed to delete the receipt product.");
    }
}