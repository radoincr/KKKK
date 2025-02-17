using INV.Domain.Entities.SupplierEntity;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Suppliers.SupplierPofile;

public partial class ProfileOverview
{
    [Parameter] public Supplier Supplier { get; set; }
}