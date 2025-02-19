using INV.App.Suppliers;
using INV.Domain.Entities.SupplierEntity;
using INVUIs.Models.Supplier;
using INVUIs.Suppliers;
using Microsoft.AspNetCore.Components;


namespace INVUIs.BonCommande;

public partial class SupplierCommande
{
    [Parameter] public EventCallback<SupplierModel> OnSupplier { get; set; }

    [Inject] private ISupplierService supplierService { get; set; }
    private SupplierSelector addSupplier = new SupplierSelector();
    private SupplierModel supplierModel = new SupplierModel();
    private List<SupplierInfo> aaa = new List<SupplierInfo>();
    private List<Supplier> suppliers = new List<Supplier>();
    private EventCallback<SupplierInfo> supplierSelected;
    private SupplierForm supplierSelector = new SupplierForm();
    public SupplierModel sup;
    private bool SupplierSelected = false;
    private bool controlDisabled = false;

    private async Task SupplierSelectednew(SupplierInfo supplierInfo)
    {
        sup = new SupplierModel
        {
            ID = supplierInfo.ID,
            NameSupplier = supplierInfo.Name,
            NameCompany = supplierInfo.CompanyName,
            Email = supplierInfo.Email,
            Phone = supplierInfo.Phone,
            Address = supplierInfo.Address,
            NameAccount = supplierInfo.AccountName,
            RC = supplierInfo.RC,
            ART = supplierInfo.ART,
            NIS = supplierInfo.NIS,
            NIF = supplierInfo.NIF,
            BankAgency = supplierInfo.BankAgency,
            RIB = supplierInfo.RIB
        };
        SupplierSelected = true;
        StateHasChanged();
        await OnSupplier.InvokeAsync(sup);
    }

    private async Task OnSupplierSelected(ChangeEventArgs e)
    {
        string selectedSupplierId = e.Value.ToString();

        if (Guid.TryParse(selectedSupplierId, out Guid supplierGuid))
        {
            var supplierEntity = suppliers.FirstOrDefault(s => s.ID == supplierGuid);

            if (supplierEntity != null)
            {
                supplierModel = FetchSupplierModel(supplierEntity);
                SupplierSelected = true;
            }
            else
            {
                supplierModel = null;
                SupplierSelected = false;
            }
        }
        else
        {
            supplierModel = new SupplierModel();
            SupplierSelected = false;
        }
    }

    private SupplierModel FetchSupplierModel(Supplier? supplierEntity)
    {
        if (supplierEntity == null)
        {
            return null;
        }

        return new SupplierModel
        {
            ID = supplierEntity.ID,
            NameSupplier = supplierEntity.SupplierName,
            NameCompany = supplierEntity.CompanyName,
            NameAccount = supplierEntity.AccountName,
            Address = supplierEntity.Address,
            Phone = supplierEntity.Phone,
            Email = supplierEntity.Email,
            RC = supplierEntity.RC,
            ART = supplierEntity.ART,
            NIF = supplierEntity.NIF,
            NIS = supplierEntity.NIS,
            RIB = supplierEntity.RIB,
            BankAgency = supplierEntity.BankAgency
        };
    }

    protected override async Task OnInitializedAsync()
    {
        aaa = await supplierService.GetAllSupplier();
        await Suppliers();
    }

    public async Task Suppliers()
    {
        
    }
}