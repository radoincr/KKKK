using System.Security.Principal;
using Azure.Core;
using INV.App.IOrderDetailServices;
using INV.App.Products;
using INV.App.PurchaseOrders;
using INV.App.Suppliers;
using INV.Domain.Entity.OrderDetailsEntity;
using INV.Domain.Entity.ProductEntity;
using INV.Domain.Entity.PurchaseOrderEntity;
using INV.Domain.Entity.SupplierEntity;
using INVUIs.Suppliers;
using Microsoft.AspNetCore.Components;
using Web.Components.Layout.Toast;
using Web.Models.authorityModel;
using Web.Models.PurchaseModel;
using Web.Models.SupplierModel;

namespace Web.Components.Pages;

public partial class PurchaseOrderPage
{
    [Inject] private NavigationManager navigationManager { get; set; }
    [Inject] private ISupplierService supplierService { get; set; }

    [Inject] private IPurchaseOrderService purchaseOrderService { set; get; }

    [Inject] private IOrderDetailService orderDetailService { set; get; }

    [Inject] private IProductService productService { set; get; }

    ProductModel product = new ProductModel();
    SupplierModel supplierModel = new SupplierModel();
    private List<Supplier> suppliers = new List<Supplier>();
    private AuthorityModel authority = new AuthorityModel();
    private SupplierSelector supplierSelector = new SupplierSelector();

    private List<SectionItem> sectionItems = new()
    {
        new SectionItem { Title = "Definition of the economic operator" },
        new SectionItem { Title = "Characteristics of the order" }
    };

    
    private string contractingName;
    private int payingAgentCode;
    private string SelectedArticle;
    private string contractingAddress;
    private int contactInfo;
    private string firstName;
    private string companyName;
    private string actsOnBehalf;
    private string operatorAddress;
    private string email;
    private int operatorContact;
    private string registrationNumber;
    private int articleNumber;
    private int tin;
    private int statisticalNumber;
    private string bankAccount;
    private string bankAddress;
    private int delivery_time;
    private string description_article;

    private string SelectedChapter;

    private string SelectedTVA { get; set; } = "19";
    private int DeliveryTime { get; set; }

    private string SelectTVAfromproduct { get; set; }

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

    public void NavigatePage()
    {
        navigationManager.NavigateTo("Supplier");
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
        for (int i = 0; i < products.Count; i++)
        {
            products[i].Number = i + 1;
        }
    }

    private bool SupplierSelected = false;

    private async void OnSupplierSelected(ChangeEventArgs e)
    {
        string selectedSupplierId = e.Value.ToString();

        if (Guid.TryParse(selectedSupplierId, out Guid supplierGuid))
        {
            var supplierEntity = suppliers.FirstOrDefault(s => s.ID == supplierGuid);

            if (supplierEntity != null)
            {
                supplierModel = FetchSupplierModel(supplierEntity);
                SupplierSelected = true;
            }
            else
            {
                supplierModel = null;
                SupplierSelected = false;
            }
        }
        else
        {
            supplierModel = new SupplierModel();
            SupplierSelected = false;
        }
    }

    private SupplierModel FetchSupplierModel(Supplier? supplierEntity)
    {
        if (supplierEntity == null)
        {
            return null;
        }

        return new SupplierModel
        {
            ID = supplierEntity.ID,
            NameSupplier = supplierEntity.SupplierName,
            NameCompany = supplierEntity.CompanyName,
            NameAccount = supplierEntity.AccountName,
            Address = supplierEntity.Address,
            Phone = supplierEntity.Phone,
            Email = supplierEntity.Email,
            RC = supplierEntity.RC,
            ART = supplierEntity.ART,
            NIF = supplierEntity.NIF,
            NIS = supplierEntity.NIS,
            RIB = supplierEntity.RIB,
            BankAgency = supplierEntity.BankAgency
        };
    }

    protected override async Task OnInitializedAsync()
    {
        await Suppliers();
    }

    public async Task Suppliers()
    {
        //suppliers = await supplierService.GetAllSupplier();
    }

    public string bg1 { set; get; } = "Bg1";
    public string bg2 { set; get; } = "Bg2";
    public string bg3 { set; get; } = "Bg3";

    public string s1 { set; get; } = "s1";
    public string s2 { set; get; } = "s2";
    public string s3 { set; get; } = "s3";
    private string OrderService { get; set; }
    private string OrderCategory { get; set; }

    public void c()
    {
        Console.WriteLine(SelectedChapter);
    }

    private async Task OnCreate()
    {
        decimal TVA = 0;
        decimal THT = 0;
        decimal TCT = 0;
        Guid idPurchaseOrder = Guid.NewGuid();
        foreach (var pd in products)
        {
            var orderDetail = new OrderDetail()
            {
                UnitPrice = pd.UnitPrice,
                Quantity = pd.Quantity,
                TVA = pd.TVA
            };
            decimal ordertva = decimal.Parse(orderDetail.TVA);
            decimal tvaValue = (orderDetail.UnitPrice * orderDetail.Quantity * ordertva) / 100;
            TVA += tvaValue;
            decimal thtValue = orderDetail.UnitPrice * orderDetail.Quantity;
            THT += thtValue;
        }

        TCT = THT + TVA;
        var purchaseOrder = new PurchaseOrder()
        {
            ID = idPurchaseOrder,
            IDSupplier = supplierModel.ID, 
            TypeService = OrderService, 
            TypeBudget = OrderCategory,
            CompletionDelay = DeliveryTime, 
            Chapter = SelectedChapter,
            Article = SelectedArticle,
            DesignationArticle=description_article,
            Date = DateOnly.FromDateTime(DateTime.Now.Date),
            Status = "In Progress",
            TVA = TVA,
            TTC = TCT,
            THT = THT
        };

        try
        {
            await purchaseOrderService.AddPurchaseOrder(purchaseOrder);

            foreach (var selectedProduct in products)
            {
                if (selectedProduct == null)
                {
                    return;
                }

                Guid _idproduct = Guid.NewGuid();
                var newProduct = new Product()
                {
                    ID = _idproduct,
                    UnitMeasure = selectedProduct.UnitOfMeasure,
                    DefaultTVARate = "19",
                    Designation = selectedProduct.Data
                };


                await productService.AddProduct(newProduct);
                var orderDetail = new OrderDetail()
                {
                    ID = Guid.NewGuid(),
                    IDPurchaseDetail = idPurchaseOrder,
                    UnitPrice = selectedProduct.UnitPrice,
                    Quantity = selectedProduct.Quantity,
                    IdProducts = _idproduct,
                    TVA = selectedProduct.TVA
                };

                await orderDetailService.AddOrderDetail(orderDetail);
            }

            ShowToast("Success", "Purchase Order created successfully", ToastType.Success);
            await ClearForm();
        }
        catch (Exception ex)
        {
            ShowToast("Error", "Error From Purchase Order created ", ToastType.Danger);
            Console.Error.WriteLine($"{ex.Message}");
        }
    }
    private async Task ClearForm()
    {
        supplierModel = new SupplierModel();
        products = new List<ProductModel>();
        SelectedChapter = "";
        SelectedArticle = "";
        description_article = "";
        SelectedTVA = "19";
        DeliveryTime = 0;
        OrderCategory = "";
        OrderService = "";
        products=new List<ProductModel>();
        suppliers = new List<Supplier>();
    }
}