

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Models.PurchaseModel;
using Web.Components.Layout.Toast;

namespace Web.Components.Pages;

public partial class PurchasePage
{
    [Inject] private NavigationManager navigationManager { get; set; }
     private List<SectionItem> sectionItems = new()
    {
        new SectionItem { Title = "Definition of the contracting authority" },
        new SectionItem { Title = "Definition of the economic operator" },
        new SectionItem { Title = "Characteristics of the order" }
    };

    private string contractingName;
    private int payingAgentCode;
    private string contractingAddress;
    private int contactInfo;
    private string firstName;
    private string companyName;
    private string actsOnBehalf;
    private string operatorAddress;
    private int operatorContact;
    private string registrationNumber;
    private int articleNumber;
    private int tin;
    private int statisticalNumber;
    private string bankStatement;
    private int delivery_time;

    private void ToggleSection(SectionItem sectionItem)
    {
        sectionItem.IsVisible = !sectionItem.IsVisible;
    }

    private string GetSectionIcon(bool isVisible)
    {
        return isVisible ? "▼" : "►";
    }

    public class SectionItem
    {
        public string Title { get; set; }
        public bool IsVisible { get; set; } = false;
    }

    private bool isToastVisible = false;
    private ToastType toastType = ToastType.Success;
    private string toastTitle = string.Empty;
    private string toastMessage = string.Empty;

    public void navigatePage()
    {
        navigationManager.NavigateTo("Supplier");
    }

    public void Sucses()
    {
        ShowToast("Sucses", "Sucses INV !", ToastType.Success);
    }

    private void ShowToast(string title, string message, ToastType type)
    {
        toastTitle = title;
        toastMessage = message;
        toastType = type;
        isToastVisible = true;
    }

    private void CloseToast()
    {
        isToastVisible = false;
    }

    private List<ProductModel> products = new();

    private void AddProduct()
    {
        int nextNumber = products.Count + 1;
        products.Add(new ProductModel { Number = nextNumber });
    }

    private void DeleteProduct(ProductModel product)
    {
        products.Remove(product);
        for (int i = product.Number; i <= products.Count; i++)
        {
        }
    }
}