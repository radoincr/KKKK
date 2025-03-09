namespace INV.App.IGeneratePdfServices;

public interface IGenPurchaseOrderPDF
{
    Task GeneratePurchaseOrderPdf(int purchaseOrderNumber, string outputPdfPath, string htmlTemplatePath);
}