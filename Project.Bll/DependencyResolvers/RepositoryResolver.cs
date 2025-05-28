using Microsoft.Extensions.DependencyInjection;
using Project.Bll.Managers.Abstracts;
using Project.Bll.Managers.Concretes;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Bll.DependencyResolvers
{
    public static class RepositoryResolver
    {
        public static void AddRepositoryServices(this IServiceCollection services)
        {
            //Eger siz, servislerinize bir Interface'in nasıl davranacagı yönünde bir Matematik ifade etmek isterseniz 3 seceneginiz vardır :  Servislerinizi ya Scoped, ya Transient ya da Singleton olarak entegre edersiniz...

            //services.AddSingleton => Instance alma durumunda ilgili request'te ilgili tipteki instance'i bir kez alır ve program kapanana kadar o instace'dan devam eder.

            //services.AddTransient => Instance alma durumunda ilgili request'ti karsılayan metotta aynı tipte kac parametre  varsa o kadar ayrı instance alınır...

            //services.AddScoped => Instance alma durumunda ilgili request'i karsılayan metotta aynı tipte kac tane parametre olursa olsun 1 kez instance alınır ve request sonlandıgında garbage collector o instance'i Ram'den kaldırır...

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();    
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IAppRoleRepository, AppRoleRepository>();
            services.AddScoped<IAppUserRoleRepository, AppUserRoleRepository>();
            services.AddScoped<IAppUserProfileRepository, AppUserProfileRepository>();
        }
    }
}
