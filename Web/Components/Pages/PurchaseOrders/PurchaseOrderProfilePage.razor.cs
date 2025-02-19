using Microsoft.AspNetCore.Components;

namespace Web.Components.Pages.PurchaseOrders;

public partial class PurchaseOrderProfilePage
{
    [Parameter] public Guid Id { get; set; }
}