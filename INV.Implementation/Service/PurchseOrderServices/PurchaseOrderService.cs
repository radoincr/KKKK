using System.Globalization;
using System.Net;
using System.Text;
using App.IPurchaseOrderServices;
using Entity.ProductPDF;
using Entity.PurchaseOrderEntity;
using Entity.SupplierEntity;
using Interface.PurchaseOrderStorage;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using pdf.Components.Service;
using Humanizer;
using Humanizer.Localisation;
using FarsiLibrary.Utils;
using Service.MyToolServices;

namespace Service.PurchseOrderServices;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderStorage purchaseOrderStorage;

    public PurchaseOrderService(IPurchaseOrderStorage purchaseOrderStorage)
    {
        this.purchaseOrderStorage =
            purchaseOrderStorage ?? throw new ArgumentNullException(nameof(purchaseOrderStorage));
    }

    public async Task<int> AddPurchaseOrder(PurchaseOrder purchaseOrder)
    {
        return await purchaseOrderStorage.InsertPurchaseOrder(purchaseOrder);
    }

    public async Task<List<PurchaseOrder>> GetAllPurchaseOrder()
    {
        return await purchaseOrderStorage.SelectAllPurchaseOrder();
    }

    public async Task<(List<PurchaseOrder>, List<Supplier>, List<ProductPdf>)> GetPurchaseOrderDetails(
        int purchaseOrderNumber)
    {
        return await purchaseOrderStorage.SelectPurchaseOrderDetails(purchaseOrderNumber);
    }

    public async Task GeneratePurchaseOrderPdf(int purchaseOrderNumber, string outputPdfPath, string htmlTemplatePath)
    {
        try
        {
            var (purchaseOrders, suppliers, orderDetails) =
                await purchaseOrderStorage.SelectPurchaseOrderDetails(purchaseOrderNumber);

            if (!purchaseOrders.Any())
                throw new InvalidOperationException($"No data found for Purchase Order #{purchaseOrderNumber}.");

            string htmlContent = await File.ReadAllTextAsync(htmlTemplatePath);

            string updatedHtmlContent = InjectPurchaseOrderToHtml(htmlContent, purchaseOrders, suppliers, orderDetails);

            await new BrowserFetcher().DownloadAsync();

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(
                $"<html dir='rtl'><head><meta charset='utf-8'></head><body>{updatedHtmlContent}</body></html>",
                new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Load } });
            await page.AddStyleTagAsync(new AddTagOptions
            {
                Content = @"
            body { font-family: 'Amiri', 'Tajawal', 'Arial', sans-serif; direction: rtl; text-align: right; }
            table { width: 100%; border-collapse: collapse; }
            td, th { border: 1px solid black; padding: 5px; }
        "
            });

            await page.PdfAsync(outputPdfPath, new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions { Top = "20px", Bottom = "20px", Left = "20px", Right = "20px" }
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }


    private static string InjectPurchaseOrderToHtml(string htmlContent, List<PurchaseOrder> purchaseOrders,
        List<Supplier> suppliers, List<ProductPdf> productpdfs)
    {
        var sb = new StringBuilder(htmlContent);
        ArabicNumber conArabicNumber = new ArabicNumber();
        if (purchaseOrders.Any())
        {
            var po = purchaseOrders.First();
            sb.Replace("{{OrderNumber}}", po.Number.ToString());
            sb.Replace("{{OrderDate}}", po.Date.ToString("yyyy-MM-dd"));
            sb.Replace("{{OrderStatus}}", po.Status);
            sb.Replace("{{TypeBudget}}", po.TypeBudget);
            sb.Replace("{{TypeService}}", po.TypeService);
            sb.Replace("{{Article}}", po.Article);
            sb.Replace("{{Chapter}}", po.Chapter);
            sb.Replace("{{CompletionDelay}}", po.CompletionDelay.ToString());
            sb.Replace("{{TotalHT}}", po.THT.ToString("F"));
            sb.Replace("{{TVA}}", po.TVA.ToString("F"));
            sb.Replace("{{TotalTTC}}", po.TTC.ToString("F"));
            string arabicWords = conArabicNumber.arabicNumber((double)po.TTC);
            sb.Replace("{{TotalTTCArabic}}", arabicWords);
            
            string serviceOptions = $@"
            <li><span class='square'>{(po.TypeService == "Bg1" ? "✔" : "   ")}</span>  اشغال </li>
            <li><span class='square'>{(po.TypeService == "Bg2" ? "✔" : "   ")}</span>لوازم </li>
            <li><span class='square'>{(po.TypeService == "Bg3" ? "✔" : "   ")}</span> خدمات </li>";
            sb.Replace("{{TypeServiceCheckboxes}}", serviceOptions);
            
            string budgetOptions = $@"
            <li><span class='square'>{(po.TypeBudget == "s1" ? "x" : "   ")}</span> نفقات التسيير </li>
            <li><span class='square'>{(po.TypeBudget == "s2" ? "x" : "   ")}</span> نفقات التخهيز </li>
            <li><span class='square'>{(po.TypeBudget == "s3" ? "x" : "   ")}</span> نفقات اخرى </li>";
            sb.Replace("{{TypeBudgetCheckboxes}}", budgetOptions);
        }

        if (suppliers.Any())
        {
            var supplier = suppliers.First();
            sb.Replace("{{SupplierName}}", supplier.SupplierName);
            sb.Replace("{{CompanyName}}", supplier.CompanyName);
            sb.Replace("{{AccountName}}", supplier.AccountName);
            sb.Replace("{{Email}}", supplier.Email);
            sb.Replace("{{Phone}}", supplier.Phone);
            sb.Replace("{{RC}}", supplier.RC);
            sb.Replace("{{NIS}}", supplier.NIS.ToString());
            sb.Replace("{{NIF}}", supplier.NIF.ToString());
            sb.Replace("{{RIB}}", supplier.RIB);
            sb.Replace("{{BankAgency}}", supplier.BankAgency);
        }

        if (productpdfs.Any())
        {
            var tableContent = new StringBuilder();
            int i = 1;
            foreach (var item in productpdfs)
            {
                tableContent.Append($@"
                <tr>
                    <td>{i++}</td>
                    <td>{item.Designation}</td>
                    <td>{item.Unitmesure}</td>
                    <td>{item.Quantity}</td>
                    <td>{item.Price}</td>
                    <td>{item.Quantity * item.Price}</td>

                </tr>");
            }

            sb.Replace("{{OrderDetails}}", tableContent.ToString());
        }
        return sb.ToString();
    }
}