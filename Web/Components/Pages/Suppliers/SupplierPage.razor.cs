using INV.App.Purchases;
using INV.Web.Services.Suppliers;
using Microsoft.AspNetCore.Components;

namespace INV.Web.Components.Pages.Suppliers
{
    public partial class SupplierPage
    {
        [Parameter] public Guid id { get; set; }
        private SupplierDetail Supplier { get; set; } 
        private List<PurchaseOrderInfo> purchases;
        [Inject] public IAppSupplierService serviceSupplier { set; get; }

       

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Supplier = await serviceSupplier.GetSupplierDetail(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}