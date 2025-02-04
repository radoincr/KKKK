using App.ISupplierService;
using Interface.SupplierStorages;
using Service.SupplierServices;
using Storage.SupplierStorages;
using Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<ISupplierStorage, SupplierStorage>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
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