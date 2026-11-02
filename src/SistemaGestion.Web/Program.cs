var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<SistemaGestion.Web.Services.CatalogoEnMemoria>();
builder.Services.AddSingleton<SistemaGestion.Web.Services.ClientesEnMemoria>();
builder.Services.AddSingleton<SistemaGestion.Web.Services.VentasEnMemoria>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
