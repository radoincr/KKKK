using INV.App.Suppliers;
using INV.Domain.Entities.SupplierEntity;
using INVUIs.Models.Supplier;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Suppliers
{
    public partial class SupplierForm
    {
        [Inject] public ISupplierService SupplierService { get; set; }
        SupplierModel newSupplier = new SupplierModel();
        private bool displayModal = false;
        private void close()
        {
            newSupplier = new SupplierModel(); // Reset instead of null
            displayModal = false;
            StateHasChanged(); // Force UI re-render
        }

    

        public void ShowModal()
        {
            displayModal = true;
            StateHasChanged();
        }

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
             


            }
            catch (Exception ex)
            {
              
            }
        }
     /*   private void ShowToast(string title, string message, ToastType type)
        {
            toastTitle = title;
            toastMessage = message;
            toastType = type;
            isToastVisible = true;
        }
        public void CloseToast()
        {
            isToastVisible = false;
        }*/
        private async Task ClearForm()
        {
            newSupplier = new SupplierModel();
        }
    }
}
