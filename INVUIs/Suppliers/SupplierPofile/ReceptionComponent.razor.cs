using INV.Domain.Entities.SupplierEntity;

namespace INVUIs.Suppliers.SupplierPofile;

public partial class ReceptionComponent
{
    public Supplier sup { set; get; } = new();
    private string ErrorMessage { get; set; }

    private async Task OK()
    {
    }
}