using INV.App.Suppliers;
using INV.Domain.Entities.SupplierEntity;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Suppliers.SupplierPofile;

public partial class EditProfileForm
{
    [Parameter] public Supplier Supplier { get; set; }
    [Inject] public ISupplierService supplierService { set; get; }
    [Inject] public NavigationManager navigation { set; get; }
    private Supplier supplierEdit { get; set; } = new();

    protected override void OnParametersSet()
    {
        supplierEdit = new Supplier
        {
            ID = Supplier.ID,
            SupplierName = Supplier.SupplierName,
            CompanyName = Supplier.CompanyName,
            Address = Supplier.Address,
            Email = Supplier.Email,
            Phone = Supplier.Phone,
            RC = Supplier.RC,
            ART = Supplier.ART,
            NIF = Supplier.NIF,
            NIS = Supplier.NIS,
            RIB = Supplier.RIB,
            BankAgency = Supplier.BankAgency,
            AccountName = Supplier.AccountName
        };
    }

    private async Task UpdateSupplier()
    {
        supplierEdit.ID = Supplier.ID;
        await supplierService.SetSupplier(supplierEdit);
        StateHasChanged();
        await Task.Delay(2000);
        navigation.NavigateTo(navigation.Uri, forceLoad: true);
    }
}

