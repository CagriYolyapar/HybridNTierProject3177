using Project.Bll.DependencyResolvers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();





builder.Services.AddDbContextService(); //İşte bu noktadan sonra bir Veritabanı testi yapılır... Farklı katmanlar üzerinden EF Core'u CodeFirst ile ayaga kaldırmaya calısırsanız sistem sizden Set as Startup olarak ayarlanmıs projede Microsoft.EntityFrameworkCore.Design kütüphanesini isteyecektir...




builder.Services.AddIdentityService();

builder.Services.AddRepositoryServices();
builder.Services.AddManagerServices();

builder.Services.AddHttpClient(); //Eger bir API consume edilecekse HTTP client tarafında oldugumuzu Middleware'e belirtmeliyiz...


builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(x =>
{
    x.IdleTimeout = TimeSpan.FromDays(1);
    x.Cookie.HttpOnly = true;
    x.Cookie.IsEssential  = true;   
});



builder.Services.ConfigureApplicationCookie(x =>
{
    x.AccessDeniedPath = "/Home/SignIn"; //Authorization (yetki durumu yoksa) o noktada kullanıcının yönlendirilecegi path
    x.LoginPath = "/Home/SignIn"; //Authenticaton yoksa ...
});



WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area}/{controller=Category}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shopping}/{action=Index}/{id?}");

app.Run();
