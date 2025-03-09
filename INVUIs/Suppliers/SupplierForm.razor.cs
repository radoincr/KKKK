using INV.App.Suppliers;
using INV.Domain.Entities.Suppliers;
using INV.Domain.Shared;
using INVUIs.Suppliers.Models;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Suppliers;

public partial class SupplierForm : ComponentBase
{
    [Parameter] public string CreateButtonLabel { get; set; } = "Create";
    [Parameter] public EventCallback<SupplierInfo> OnSupplierCreated { get; set; }
    [Inject] public ISupplierService SupplierService { get; set; }
    [Inject] public NavigationManager navigationManager { get; set; }

    private bool displayModal = false;
    private SupplierModel newSupplier = new();
    private Result result;
    private string success = string.Empty;
    [Parameter] public EventCallback<SupplierInfo> OnSave { get; set; }

    public void closeModel()
    {
        newSupplier = new SupplierModel();
        displayModal = false;
        StateHasChanged();
    }


    public void ShowModal()
    {
        displayModal = true;
        StateHasChanged();
    }

    private async Task OnCreate()
    {
        var sup = new Supplier
        {
            Id = Guid.NewGuid(),
            ManagerName = newSupplier.NameSupplier,
            CompanyName = newSupplier.NameCompany,
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

        result = await SupplierService.AddSupplier(sup);

        if (result.IsSuccess)
        {
            var createdSupplierInfo = new SupplierInfo
            {
                ID = sup.Id,
                Name = sup.ManagerName,
                CompanyName = sup.CompanyName,
                Email = sup.Email,
                Address = sup.Address,
                Phone = sup.Phone,
                ART = sup.ART,
                NIF = sup.NIF,
                RC = sup.RC,
                NIS = sup.NIS,
                RIB = sup.RIB,
                BankAgency = sup.BankAgency
            };
            await OnSupplierCreated.InvokeAsync(createdSupplierInfo);
        }

        closeModel();
        success = "The supplier has been added successfully";
        await ClearForm();
    }

    private async Task ClearForm()
    {
        newSupplier = new SupplierModel();
    }
}