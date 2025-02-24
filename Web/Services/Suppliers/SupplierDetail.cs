using INV.App.Purchases;
using INV.Domain.Entities.Suppliers;

namespace INV.Web.Services.Suppliers;

public class SupplierDetail : ISupplier
{
    public Guid Id { set; get; }
    public string CompanyName { set; get; }
    public string ManagerName { get; set; }
    public string Address { set; get; }
    public string Phone { set; get; }
    public string Email { set; get; }
    public string RC { set; get; }
    public string NIS { set; get; }
    public string ART { set; get; }
    public string RIB { set; get; }
    public string NIF { set; get; }
    public string BankAgency { set; get; }
    public SupplierState State { set; get; }
    public List<PurchaseOrderInfo> Purchases { get; set; } = new ();
}