using App.IOrderDetailServices;
using App.IProductServices;
using App.IPurchaseOrderServices;
using App.ISupplierService;
using Intefrace.ProductStorages;
using Interface.OrderDetailsStorage;
using Interface.PurchaseOrderStorage;
using Interface.SupplierStorages;
using Service.OrderDetailServices;
using Service.ProductServices;
using Service.PurchseOrderServices;
using Service.SupplierServices;
using Storage.OrderDetailsStorage;
using Storage.ProductStorage;
using Storage.PurchaseOrderStorage;
using Storage.SupplierStorages;
using Web.Components;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<ISupplierStorage, SupplierStorage>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IOrderDetailStorage, OrderDetailStorage>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IProductStorage, ProductStorage>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPurchaseOrderStorage, PurchaseOrderStorage>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
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