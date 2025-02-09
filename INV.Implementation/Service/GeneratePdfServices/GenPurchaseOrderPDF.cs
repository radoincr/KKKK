using System.Text;
using INV.App.IGeneratePdfServices;
using INV.Domain.Entity.ProductPDF;
using INV.Domain.Entity.PurchaseOrderEntity;
using INV.Domain.Entity.SupplierEntity;
using INV.Implementation.Service.MyToolServices;
using INV.Infrastructure.Storage.PurchaseOrderStorages;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace INV.Implementation.Service.GeneratePdfServices;

public class GenPurchaseOrderPDF : IGenPurchaseOrderPDF
{
    private readonly IPurchaseOrderStorage purchaseOrderStorage;

    public GenPurchaseOrderPDF(IPurchaseOrderStorage purchaseOrderStorage)
    {
        this.purchaseOrderStorage = purchaseOrderStorage;
    }

    public async Task GeneratePurchaseOrderPdf(int purchaseOrderNumber, string outputPdfPath, string htmlTemplatePath)
    {
        try
        {
            var (purchaseOrders, suppliers, orderDetails) =
                await purchaseOrderStorage.SelectPurchaseOrderDetails(purchaseOrderNumber);

            if (!purchaseOrders.Any())
                throw new InvalidOperationException($"Nember vide{purchaseOrderNumber}!!");

            string htmlContent = await File.ReadAllTextAsync(htmlTemplatePath);

            string updatedHtmlContent = injectPurchaseOrderToHtml(htmlContent, purchaseOrders, suppliers, orderDetails);

            await new BrowserFetcher().DownloadAsync();

            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
            });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(
                $"<html dir='rtl'><head><meta charset='utf-8'></head><body>{updatedHtmlContent}</body></html>",
                new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Load } });
            await page.AddStyleTagAsync(new AddTagOptions
            {
                Content = @"
        body { font-family: 'Amiri', 'Tajawal', 'Arial', sans-serif; direction: rtl; text-align: right; }
        
        .red { 
            width: 100%; 
            border-collapse: separate; 
            border-spacing: 0; 
            border-radius: 10px; 
            overflow: hidden; 
            border: 1px solid black; 
        }
        .header { 
            background-color: #e2e2e2;  
            font-weight: bold; 
            text-align: center; 
            padding: 4px; 
            font-size: 18px;
        }
    "
            });

            await page.PdfAsync(outputPdfPath, new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions { Top = "20px", Bottom = "20px", Left = "20px", Right = "20px" },
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw;
        }
    }

    private static string injectPurchaseOrderToHtml(string htmlContent, List<PurchaseOrder> purchaseOrders,
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
            <li><span class='square'>{(po.TypeBudget == "s1" ? "✔" : "   ")}</span> نفقات التسيير </li>
            <li><span class='square'>{(po.TypeBudget == "s2" ? "✔" : "   ")}</span> نفقات التجهيز </li>
            <li><span class='square'>{(po.TypeBudget == "s3" ? "✔" : "   ")}</span> نفقات اخرى </li>";
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
            sb.Replace("{{ART}}", supplier.ART.ToString());
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
                    <td>{item.TVA}</td>
                    <td>{item.Quantity * item.Price}</td>

                </tr>");
            }

            sb.Replace("{{OrderDetails}}", tableContent.ToString());
        }

        return sb.ToString();
    }
}