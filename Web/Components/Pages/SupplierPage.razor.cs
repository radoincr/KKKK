using INV.App.Suppliers;
using INV.Domain.Entity.SupplierEntity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Web.Components.Layout.Toast;
using Web.Models.SupplierModel;

namespace Web.Components.Pages;

public partial class SupplierPage
{
    [Inject] public ISupplierService SupplierService { get; set; }
    SupplierModel newSupplier = new SupplierModel();
    private bool isToastVisible = false;
    private ToastType toastType = ToastType.Success;
    private string toastTitle = string.Empty;
    private string toastMessage = string.Empty;

    private async Task OnCreate()
    {
        try
        {
                var sup = new Supplier()
                {
                    ID = Guid.NewGuid(),
                    SupplierName = newSupplier.NameSupplier,
                    CompanyName = newSupplier.NameCompany,
                    AccountName = newSupplier.NameCompany,
                    Email = newSupplier.Email,
                    Address = newSupplier.Address,
                    Phone = newSupplier.Phone,
                    ART = newSupplier.ART,
                    NIF = newSupplier.NIF,
                    RC = newSupplier.RC,
                    NIS = newSupplier.NIS,
                    RIB = newSupplier.RIB,
                    BankAgency = newSupplier.BankAgency
                };

                await SupplierService.AddSupplier(sup);
                await ClearForm();
                ShowToast("Succses", "Succses Add Supplier", ToastType.Success);
                
            
        }
        catch (Exception ex)
        {
            ShowToast("Error", "Supplier Exsist ", ToastType.Danger);
        }
    }
    private void ShowToast(string title, string message, ToastType type)
    {
        toastTitle = title;
        toastMessage = message;
        toastType = type;
        isToastVisible = true;
    }
    public void CloseToast()
    {
        isToastVisible = false;
    }
    private async Task ClearForm()
    {
        newSupplier = new SupplierModel();
    }
}