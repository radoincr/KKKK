using INV.App.IGeneratePdfServices;
using INV.App.IOrderDetailServices;
using INV.App.Products;
using INV.App.PurchaseOrders;
using INV.App.Suppliers;
using INV.Implementation.Service.GeneratePdfServices;
using INV.Implementation.Service.OrderDetailServices;
using INV.Implementation.Service.ProductServices;
using INV.Implementation.Service.PurchseOrderServices;
using INV.Implementation.Service.Suppliers;
using INV.Infrastructure.Storage.OrderDetailsStorages;
using INV.Infrastructure.Storage.ProductsStorages;
using INV.Infrastructure.Storage.PurchaseOrderStorages;
using INV.Infrastructure.Storage.SupplierStorages;
using Web.Components;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<ISupplierStorage, SupplierStorage>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISupplierOrderService, SupplierOrderService>();
builder.Services.AddScoped<ISupplierOrderStorage, SupplierOrderStorage>();

builder.Services.AddScoped<IOrderDetailStorage, OrderDetailStorage>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IProductStorage, ProductStorage>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPurchaseOrderStorage, PurchaseOrderStorage>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IGenPurchaseOrderPDF, GenPurchaseOrderPDF>();
builder.Services.AddBlazorBootstrap();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();