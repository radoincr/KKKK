using INV.Domain.Entities.PurchaseOrders;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Orders.PurchaseOrderDetails;

public partial class PurchaseDetails
{
    [Parameter] public PurchaseOrder PurchaseOrder { get; set; }
}