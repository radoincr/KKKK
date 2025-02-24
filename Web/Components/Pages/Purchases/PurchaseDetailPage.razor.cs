using Microsoft.AspNetCore.Components;

namespace INV.Web.Components.Pages.Purchases
{
    public partial class PurchaseDetailPage
    {
        [Parameter] public Guid Id { get; set; }
    }
}