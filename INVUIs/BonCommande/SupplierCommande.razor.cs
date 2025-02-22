using INV.App.Suppliers;
using INV.Domain.Entities.PurchaseOrders;
using INV.Domain.Entities.SupplierEntity;
using INVUIs.Models.Supplier;
using INVUIs.Suppliers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;


namespace INVUIs.BonCommande;

public partial class SupplierCommande
{
    [Parameter] public EventCallback<SupplierModel> OnSupplier { get; set; }
    public EditContext editContext { get; set; }
    [Inject] private ISupplierService supplierService { get; set; }
    private SupplierSelector addSupplier = new SupplierSelector();
    private SupplierModel supplierModel = new SupplierModel();
    private List<SupplierInfo> aaa = new List<SupplierInfo>();
    private List<Supplier> suppliers = new List<Supplier>();
    private EventCallback<SupplierInfo> supplierSelected;
    private SupplierForm supplierSelector = new SupplierForm();
    public SupplierModel sup = new SupplierModel();
    private bool Display = false;
    public void Show() => Display = !Display;
    private bool SupplierSelected = false;
    private bool controlDisabled = false;

    private async Task SupplierSelectednew(SupplierInfo supplierInfo)
    {
        sup = new SupplierModel
        {
            ID = supplierInfo.ID,
            Behalf = sup.Behalf,
            NameSupplier = supplierInfo.Name,
            NameCompany = supplierInfo.CompanyName,
            Address = supplierInfo.Address,
            Phone = supplierInfo.Phone,
            Email = supplierInfo.Email,
            RC = supplierInfo.RC,
            ART = supplierInfo.ART,
            NIF = supplierInfo.NIF,
            NIS = supplierInfo.NIS,
            RIB = supplierInfo.RIB,
            BankAgency = supplierInfo.BankAgency
        };
        editContext = new EditContext(sup);

        SupplierSelected = true;
        StateHasChanged();
        await OnSupplier.InvokeAsync(sup);
    }

    public async Task SupplierPass()
    {
        await OnSupplier.InvokeAsync(sup);
    }


    protected override async Task OnParametersSetAsync()
    {
        aaa = await supplierService.GetAllSupplier();
    }

    public void Close() => Display = false;
}