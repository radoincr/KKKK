using INV.App.Purchases;
using INV.Shared;
using Microsoft.AspNetCore.Components;

namespace INVUIs.Purchases
{
    public partial class PurchaseList
    {
        [Parameter] public List<PurchaseOrderInfo> purchaseOrderInfos { get; set; }
        [Parameter] public RenderFragment Pills { get; set; }
        [Parameter] public bool ShowSupplier { get; set; } = true;
        public async Task navigatepage(Guid id) => Navigation.NavigateTo($"{PageRoutes.Purchases}/{id}");

        private string SearchTerm { get; set; } = "";
        private List<PurchaseOrderInfo> displayedItems =>
            purchaseOrderInfos.Where(i =>
                i.Number.ToString().Contains(SearchTerm) ||
                i.SupplierName.ToString().ToLower().Contains(SearchTerm.ToLower()))
                    .ToList();

    }
}