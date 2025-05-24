using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Bll.DependencyResolvers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContextService(); //İşte bu noktadan sonra bir Veritabanı testi yapılır... Farklı katmanlar üzerinden EF Core'u CodeFirst ile ayaga kaldırmaya calısırsanız sistem sizden Set as Startup olarak ayarlanmıs projede Microsoft.EntityFrameworkCore.Design kütüphanesini isteyecektir...

builder.Services.AddIdentityService();






WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
