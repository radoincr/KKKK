using App.ISupplierService;
using Entity.SupplierEntity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Web.Components.Layout.Toast;
using Web.Models.SupplierModels;

namespace Web.Components.Pages;

public partial class Supplier
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
            var editContext = new EditContext(newSupplier);
            if (!editContext.Validate())
            {
                ShowToast("Validation Error", "Please fill all required fields.", ToastType.Warning);
                return;
            }
            else
            {
                var sup = new Entity.SupplierEntity.Supplier()
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
                ShowToast("Succses", "Succses Add Supplier", ToastType.Success);
            }
        }
        catch (Exception ex)
        {
            ShowToast("Error", "Formailer is null", ToastType.Danger);
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
}