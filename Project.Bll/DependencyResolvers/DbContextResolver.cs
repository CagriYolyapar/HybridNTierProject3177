using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Dal.ContextClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Bll.DependencyResolvers
{
    public static class DbContextResolver
    {
        public static void AddDbContextService(this IServiceCollection services)
        {

           //Baska bir katmandan bir baska katmanın .json uzantılı configuration (Proje ayarları) kısmına ulasabilmeniz icin bir servis saglayıcısına ihtiyacınız var...

            ServiceProvider serviceProvider = services.BuildServiceProvider(); //ServiceProvider tipi proje ayarlarına ulasabilecek bir tiptir...(Set as Startup olarak ayarlanmıs projenin ayarlarına ulasacaktır)

            //O ayarların barındıgı tip aslında IConfiguration tipidir... ServiceProvider tipi, IConfiguration tipine yol acan bir tüneldir...Yani ServiceProvider tipi olmadan IConfiguration tipine ulasamazsınız...

            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();

            services.AddDbContext<MyContext>(x => x.UseSqlServer(configuration.GetConnectionString("MyConnection")).UseLazyLoadingProxies());
        }
    }
}
