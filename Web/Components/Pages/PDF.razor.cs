using INV.App.IGeneratePdfServices;
using INV.App.PurchaseOrders;
using INV.Domain.Entity.PurchaseOrderEntity;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Web.Components.Pages;

public partial class PDF
{
    [Inject] public IPurchaseOrderService purchaseOrderService { get; set; }
    [Inject] public IGenPurchaseOrderPDF genPurchaseOrderPdf { get; set; }

    [Inject] public IJSRuntime JS { set; get; }
    private List<PurchaseOrder> purchaseOrders = new();
    private List<PurchaseOrder> filteredOrders = new();
    private int? searchNumber;
    private DateTime? searchDate;
    private async Task GetPurchaseOrder()
    {
        if (searchDate.HasValue)
        {   DateOnly selectedDate = DateOnly.FromDateTime(searchDate.Value);
            purchaseOrders = await purchaseOrderService.GetPurchaseOrdersByDate(selectedDate);
            SearchOrders();
        }
    }

    private void SearchOrders()
    {
        DateOnly? selectedDate = searchDate.HasValue ? DateOnly.FromDateTime(searchDate.Value) : null;

        filteredOrders = purchaseOrders.Where(order =>
            (!searchNumber.HasValue || order.Number == searchNumber.Value) &&
            (!selectedDate.HasValue || order.Date == selectedDate.Value)
        ).ToList();
    }


    private async Task DownloadFile(int orderNumber)
    {
        try
        {
            string filePath = $"C:/Users/radoi/Videos/PurchaseOrder_{orderNumber}.pdf";
            string templatePath = "C://Users//radoi//OneDrive//Desktop//New folder (4)//INV//Web//Components//Pages//a.html";

            await genPurchaseOrderPdf.GeneratePurchaseOrderPdf(orderNumber, filePath, templatePath);
            var fileBytes = await File.ReadAllBytesAsync(filePath);
            await JS.InvokeVoidAsync("downloadFile", fileBytes, $"PurchaseOrder_{orderNumber}.pdf");
        }
        catch (Exception e)
        {
            Console.WriteLine($"error{e.Message}");
        }
    }
}
