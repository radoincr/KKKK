using ims.Web.Components;
using INV.App.Products;
using INV.App.PurchaseOrders;
using INV.App.Suppliers;
using INV.Implementation.Service.ProductServices;
using INV.Implementation.Service.PurchseOrderServices;
using INV.Implementation.Service.Suppliers;
using INV.Infrastructure.Storage.ProductsStorages;
using INV.Infrastructure.Storage.PurchaseProducts;
using INV.Infrastructure.Storage.SupplierStorages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ISupplierStorage, SupplierStorage>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IProductStorage, ProductStorage>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPurchaseOrderStorage, PurchaseOrderStorage>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddBlazorBootstrap();
builder.Services.AddBootstrapBlazor();

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
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();