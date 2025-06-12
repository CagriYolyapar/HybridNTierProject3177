using Microsoft.Extensions.DependencyInjection;
using Project.Bll.Managers.Abstracts;
using Project.Bll.Managers.Concretes;

namespace Project.Bll.DependencyResolvers
{
    public static class ManagerResolver
    {
        public static void AddManagerServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryManager,CategoryManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<IOrderDetailManager, OrderDetailManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<IAppUserManager, AppUserManager>();
            services.AddScoped<IAppRoleManager, AppRoleManager>();
            services.AddScoped<IAppUserRoleManager, AppUserRoleManager>();
            services.AddScoped<IAppUserProfileManager, AppUserProfileManager>();
        }
    }
}
